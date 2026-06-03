# Testing Guide - Authentication & Authorization

## Prerequisites

- Visual Studio 2026 or later
- .NET 10 SDK
- SQL Server (LocalDB or full instance)
- Postman or Thunder Client (for API testing)

---

## 1. Initial Setup

### Step 1: Verify Database Connection
1. Update `appsettings.json` with correct connection string:
```json
{
  "ConnectionStrings": {
	"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=GoBikeDB;Trusted_Connection=true;"
  }
}
```

### Step 2: Build Solution
```powershell
dotnet build
```

### Step 3: Verify Build
```
Build successful ✅
No compilation errors
All projects compiled
```

---

## 2. API Testing

### Test Scenario 1: Login as Admin

**Endpoint**: `POST https://localhost:5001/api/auth/login`

**Request Body**:
```json
{
  "username": "admin",
  "password": "admin123"
}
```

**Expected Response** (200 OK):
```json
{
  "message": "Login successful",
  "user": {
	"id": 1,
	"username": "admin",
	"fullName": "Admin User",
	"email": "admin@gobike.vn",
	"role": "Admin",
	"roleName": "Admin"
  }
}
```

**Verify**:
- ✅ Status code: 200
- ✅ Cookie set (check Response Headers → Set-Cookie)
- ✅ User data matches seeded data
- ✅ Role correctly returned

---

### Test Scenario 2: Login as Staff

**Endpoint**: `POST https://localhost:5001/api/auth/login`

**Request Body**:
```json
{
  "username": "staff",
  "password": "staff123"
}
```

**Expected Response** (200 OK):
```json
{
  "message": "Login successful",
  "user": {
	"id": 2,
	"username": "staff",
	"fullName": "Staff User",
	"email": "staff@gobike.vn",
	"role": "Staff",
	"roleName": "Staff"
  }
}
```

**Verify**:
- ✅ Status code: 200
- ✅ Role is "Staff"
- ✅ Cookie set correctly

---

### Test Scenario 3: Invalid Credentials

**Endpoint**: `POST https://localhost:5001/api/auth/login`

**Request Body**:
```json
{
  "username": "admin",
  "password": "wrongpassword"
}
```

**Expected Response** (401 Unauthorized):
```json
{
  "message": "Invalid username or password"
}
```

**Verify**:
- ✅ Status code: 401
- ✅ No cookie set
- ✅ Clear error message

---

### Test Scenario 4: Missing Credentials

**Endpoint**: `POST https://localhost:5001/api/auth/login`

**Request Body**:
```json
{
  "username": "",
  "password": ""
}
```

**Expected Response** (400 Bad Request):
```json
{
  "message": "Username and password are required"
}
```

**Verify**:
- ✅ Status code: 400
- ✅ Validation message clear

---

### Test Scenario 5: Get Profile (Authenticated)

**Endpoint**: `GET https://localhost:5001/api/auth/profile`

**Prerequisites**:
- Must be logged in (cookie from login step)

**Expected Response** (200 OK):
```json
{
  "id": 1,
  "username": "admin",
  "fullName": "Admin User",
  "email": "admin@gobike.vn",
  "role": "Admin",
  "roleName": "Admin"
}
```

**Verify**:
- ✅ Status code: 200
- ✅ User profile data returned
- ✅ Only works with valid cookie

---

### Test Scenario 6: Get Profile (Not Authenticated)

**Endpoint**: `GET https://localhost:5001/api/auth/profile`

**Prerequisites**:
- No authentication cookie

**Expected Response**:
- ❌ 401 Unauthorized
- ❌ Or redirect to login path

**Verify**:
- ✅ Correctly rejects unauthenticated request

---

### Test Scenario 7: Logout

**Endpoint**: `POST https://localhost:5001/api/auth/logout`

**Prerequisites**:
- Must be logged in

**Expected Response** (200 OK):
```json
{
  "message": "Logout successful"
}
```

**Verify**:
- ✅ Status code: 200
- ✅ Cookie cleared (Set-Cookie with empty value)
- ✅ Subsequent requests are unauthorized

---

## 3. WebUI Testing

### Test Scenario 1: Access Login Page

**URL**: `https://localhost:5002/Account/Login`

**Expected Result**:
- ✅ Login form displayed
- ✅ Username field present
- ✅ Password field present
- ✅ Submit button present
- ✅ Demo credentials hint shown

**Verify**:
- Page loads without errors
- All form elements visible
- Bootstrap styling applied

---

### Test Scenario 2: Login with Admin Credentials

**Steps**:
1. Navigate to `https://localhost:5002/Account/Login`
2. Enter username: `admin`
3. Enter password: `admin123`
4. Click "Login"

**Expected Result**:
- ✅ Redirects to home page
- ✅ No error messages
- ✅ Can see navbar with admin badge
- ✅ Session cookie set

**Verify**:
- Login successful message shown (if redirects to dashboard)
- User profile appears in navbar
- Admin layout used (dark theme with ADMIN badge)

---

### Test Scenario 3: Login with Staff Credentials

**Steps**:
1. Navigate to `https://localhost:5002/Account/Login`
2. Enter username: `staff`
3. Enter password: `staff123`
4. Click "Login"

**Expected Result**:
- ✅ Redirects to home page
- ✅ Staff layout used (secondary theme with STAFF badge)
- ✅ Different navbar styling than admin

**Verify**:
- Different visual theme vs admin
- Staff badge shown
- Session established

---

### Test Scenario 4: Login with Invalid Credentials

**Steps**:
1. Navigate to `https://localhost:5002/Account/Login`
2. Enter username: `admin`
3. Enter password: `wrongpassword`
4. Click "Login"

**Expected Result**:
- ✅ Stays on login page
- ✅ Error message shown: "Invalid username or password"
- ✅ Form fields not cleared (user can retry)

**Verify**:
- Error message displays
- User can correct and retry
- No redirect on failure

---

### Test Scenario 5: Layout Selection

**Test Admin Layout**:
1. Login as admin
2. Check page source or inspect navbar

**Verify**:
- Uses `_LayoutAdmin.cshtml`
- Dark theme applied
- "ADMIN" badge visible

**Test Staff Layout**:
1. Login as staff
2. Check page source or inspect navbar

**Verify**:
- Uses `_LayoutStaff.cshtml`
- Secondary theme applied
- "STAFF" badge visible

---

### Test Scenario 6: Logout

**Steps**:
1. Login as any user
2. Click user dropdown in navbar
3. Click "Logout"

**Expected Result**:
- ✅ Redirects to login page
- ✅ Session cookie cleared
- ✅ Cannot access protected pages

**Verify**:
- Logout successful
- Cannot navigate to protected areas
- Must login again

---

### Test Scenario 7: Access Protected Page While Logged Out

**Steps**:
1. Ensure you're logged out
2. Try to navigate to `https://localhost:5002/protected-page`

**Expected Result**:
- ✅ Redirects to login page
- ✅ Shows access denied or login required

**Verify**:
- Unauthenticated users cannot access protected pages
- Redirected to login

---

## 4. Claims Testing

### Verify Claims in Cookie

**Using Postman**:
1. Login and capture the authentication cookie
2. Decode the cookie value in JWT debugger
3. Verify claims present:
   - `sub` (NameIdentifier) = User ID
   - `name` (Name) = Username
   - `email` (Email) = Email address
   - `given_name` (GivenName) = Full Name
   - `role` (Role) = User Role

**Expected Claims**:
```
{
  "sub": "1",           // User ID
  "name": "admin",      // Username
  "email": "admin@gobike.vn",
  "given_name": "Admin User",
  "role": "Admin"
}
```

---

## 5. Authorization Testing

### Test Admin-Only Endpoint

**Assuming** you create an admin-only endpoint:
```csharp
[Authorize(Roles = "Admin")]
[HttpPost("api/motorcycles")]
public async Task<IActionResult> Create() { }
```

**Test with Admin**:
- ✅ Login as admin
- ✅ POST to endpoint → Success (200/201)

**Test with Staff**:
- ✅ Login as staff
- ✅ POST to endpoint → Forbidden (403)

**Test Unauthenticated**:
- ✅ No login
- ✅ POST to endpoint → Unauthorized (401)

---

### Test Staff-Allowed Endpoint

**Assuming** you create a staff-allowed endpoint:
```csharp
[Authorize(Roles = "Admin,Staff")]
[HttpPut("api/rentals/{id}/complete")]
public async Task<IActionResult> Complete() { }
```

**Test with Admin**:
- ✅ Login as admin
- ✅ PUT to endpoint → Success

**Test with Staff**:
- ✅ Login as staff
- ✅ PUT to endpoint → Success

**Test Unauthenticated**:
- ✅ No login
- ✅ PUT to endpoint → Unauthorized (401)

---

## 6. Cookie Management Testing

### Verify Cookie Properties

**In Browser DevTools** (F12):
1. Login to WebUI
2. Go to Application → Cookies → https://localhost:5002
3. Find cookie: `.AspNetCore.Cookies`

**Verify Properties**:
- ✅ Secure flag: ✓ (HTTPS only)
- ✅ HttpOnly: ✓ (Not accessible via JS)
- ✅ SameSite: Lax or Strict
- ✅ Expires: ~7 days from login
- ✅ Path: /

---

### Verify Sliding Expiration

**Steps**:
1. Login and note cookie expiration time
2. Wait 30 minutes
3. Make a request (view a page)
4. Check cookie expiration time again

**Expected Result**:
- ✅ Expiration time extended by 7 days from current time
- ✅ User remains logged in

---

## 7. Error Handling Testing

### Test Various Error Scenarios

| Scenario | Action | Expected | Status |
|----------|--------|----------|--------|
| Empty username | Login with "" | Validation error | 400 |
| Empty password | Login with "" | Validation error | 400 |
| Non-existent user | Login with "xyz" | Invalid credentials | 401 |
| Wrong password | Login with wrong pwd | Invalid credentials | 401 |
| Inactive user | Login (if inactive) | Account inactive | 401 |
| No cookie access protected | Access w/o login | Unauthorized | 401 |
| Expired cookie | Very old cookie | Unauthorized | 401 |

---

## 8. Performance Testing

### Test Login Performance

**Using Postman Runner**:
1. Create a collection with login request
2. Run 100 iterations
3. Measure response times

**Expected**:
- ✅ Average response: < 500ms
- ✅ 95th percentile: < 1000ms
- ✅ No timeouts

---

## 9. Security Testing

### Test Password Security

**Never Use**:
- ❌ Plain text passwords in logs
- ❌ Passwords in URLs
- ❌ Passwords in response bodies
- ❌ Passwords in headers

**Always Use**:
- ✅ HTTPS/TLS encryption
- ✅ BCrypt hashing
- ✅ HTTP-only cookies
- ✅ Secure session management

**Verify**:
1. Check logs - no passwords visible
2. Check network traffic - uses HTTPS
3. Check cookies - marked HttpOnly

---

### Test Session Fixation Prevention

**Steps**:
1. Get session cookie before login
2. Login successfully
3. Check if cookie changed

**Expected**:
- ✅ New cookie issued after login
- ✅ Old cookie invalidated

---

## 10. Checklist for Complete Testing

- [ ] API login endpoint works
- [ ] API logout endpoint works
- [ ] API profile endpoint works
- [ ] Invalid credentials rejected
- [ ] Missing credentials rejected
- [ ] WebUI login page displays
- [ ] WebUI login succeeds
- [ ] WebUI logout succeeds
- [ ] Admin layout displays for admin
- [ ] Staff layout displays for staff
- [ ] Claims created correctly
- [ ] Cookie has secure properties
- [ ] Sliding expiration works
- [ ] Protected pages require auth
- [ ] Role-based access enforced
- [ ] Error messages clear
- [ ] No passwords in logs
- [ ] HTTPS enforced
- [ ] Performance acceptable
- [ ] Session fixation prevented

---

## 11. Troubleshooting Common Issues

### Issue: "Cannot find login page"
**Solution**:
- Check file path: `WebUI/Pages/Account/Login.cshtml`
- Verify Program.cs has `MapRazorPages()`
- Clear browser cache

### Issue: "Invalid credentials" for correct password
**Solution**:
- Verify user seeded: Check database
- Verify BCrypt workFactor matches: 11
- Check password hasn't been changed

### Issue: "Cookie not being set"
**Solution**:
- Verify HTTPS is used (localhost:5001/5002)
- Check browser accepts cookies
- Verify cookie policy configured

### Issue: "401 Unauthorized on protected endpoint"
**Solution**:
- Verify user logged in (cookie present)
- Verify cookie not expired
- Verify `[Authorize]` attribute added
- Check middleware order in Program.cs

### Issue: "Layout not switching for roles"
**Solution**:
- Verify claims created in AuthController
- Verify AuthorizationHelper.GetLayoutPath() called
- Verify _LayoutAdmin.cshtml exists
- Check file paths are correct

---

**End of Testing Guide**

For additional help, refer to `QUICK_REFERENCE.md` or `AUTHENTICATION_IMPLEMENTATION.md`
