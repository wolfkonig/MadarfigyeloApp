# Security Risks

Security risks identified in the solution, ordered by severity.

## 1. Critical — Production API traffic uses cleartext HTTP
- `MauiProgram.CurrentEnvironment` is hardcoded to `Environment.Dev`
- In that branch, all API clients use `Constants.BaseUrlHttp`
- `Constants.BaseUrlHttp = "http://wolfkonig-001-site1.site4future.com/api"`
- Android explicitly permits cleartext traffic to `site4future.com` via `network_security_config.xml`
- Impact: credentials, tokens, and all app data can be intercepted or modified by a network attacker

## 2. Critical — Hardcoded credential in source
- `API/BasicAuthHandler.cs` contains a literal username/password pair
- Even if not currently wired, the secret is in the repository and app source
- Impact: secret leakage, unauthorized upstream access, credential reuse risk

## 3. High — Google Maps API key embedded in Android manifest
- `Platforms/Android/AndroidManifest.xml` contains a full Google Maps API key
- Impact: key extraction from app package, quota abuse, billing exposure if restrictions are weak

## 4. High — User password is persistently stored for token refresh
- `UserService` stores the password in secure storage
- `TokenAuthHandler` reads the stored password and silently re-authenticates when token expires
- Impact: expands blast radius of device compromise; password retention is much riskier than storing only refresh material

## 5. High — Android backups are enabled
- `android:allowBackup="true"` in `AndroidManifest.xml`
- App also stores auth metadata in preferences and secrets in secure storage usage paths
- Impact: app data may be included in device/cloud/adb backup flows depending on platform/device behavior

## 6. Medium — Sensitive API responses are cached on device in plaintext app storage
- `CachingHandler` stores raw JSON responses with `MonkeyCache.FileStore`
- Cached data includes odutelep, odu, and latogatas data
- Impact: local data exposure on rooted/debuggable/backup-accessible devices; stale sensitive data persists after logout unless cache is fully cleared

## 7. Medium — Backend/internal error details are exposed to users and logs
- `ApiServiceBase.HandleResponseAsync` appends `apiResponse.Error.Message` into alerts
- `UserService` logs `authResponse?.Error?.Content` and user email on failures
- `ConsoleLogger` writes directly to console output
- Impact: information disclosure, easier enumeration/debugging for an attacker, possible leakage of server internals or response bodies

## 8. Medium — No visible certificate pinning or stronger transport validation
- HTTPS exists in constants, but production path currently uses HTTP
- Even if switched to HTTPS later, there is no pinning or stricter trust policy
- Impact: weaker protection against certain TLS interception scenarios

## 9. Low — Duplicate iOS location usage keys / configuration hygiene issues
- `Info.plist` defines `NSLocationWhenInUseUsageDescription` twice
- Impact: not a direct exploit by itself, but indicates platform config drift and increases the chance of shipping incorrect security/privacy settings

## 10. Low — Cache fallback can serve stale data after network failures
- `CachingHandler` returns cached content on exceptions and offline states
- Impact: mostly integrity/business risk rather than direct security compromise, but can cause users to act on outdated data

## Most urgent fixes
- Switch production clients to HTTPS immediately
- Remove cleartext Android allowance for production
- Delete hardcoded credentials from source and rotate them
- Stop storing the user password; use refresh tokens or forced re-login
- Disable or tightly scope Android backup
- Restrict/rotate the Google Maps API key
- Review cached data and log/error disclosure paths
