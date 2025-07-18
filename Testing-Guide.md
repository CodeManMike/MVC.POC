# MVC POC Testing Guide

## Quick Start

The application should now be running at: `https://localhost:7xxx` (check console output for exact port)

## Manual Testing with Browser/Postman

### 1. Products API Endpoints

#### Get All Products
```http
GET https://localhost:7xxx/api/products
```
**Expected Response:**
```json
{
  "success": true,
  "message": "Products retrieved successfully",
  "data": [
    {
      "id": 1,
      "name": "Laptop Pro",
      "description": "High-performance laptop for professionals",
      "price": 1299.99,
      "category": "Electronics",
      "stockQuantity": 15,
      "isActive": true,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z"
    }
  ],
  "errors": null,
  "timestamp": "2024-01-01T12:00:00Z"
}
```

#### Get Product by ID
```http
GET https://localhost:7xxx/api/products/1
```

#### Get Products by Category
```http
GET https://localhost:7xxx/api/products/category/Electronics
```

#### Search Products
```http
GET https://localhost:7xxx/api/products/search?searchTerm=laptop
```

#### Get Low Stock Products
```http
GET https://localhost:7xxx/api/products/low-stock?threshold=10
```

#### Create New Product
```http
POST https://localhost:7xxx/api/products
Content-Type: application/json

{
  "name": "Wireless Keyboard",
  "description": "Ergonomic wireless keyboard with backlight",
  "price": 79.99,
  "category": "Electronics",
  "stockQuantity": 20
}
```

#### Update Product
```http
PUT https://localhost:7xxx/api/products/1
Content-Type: application/json

{
  "name": "Laptop Pro Updated",
  "description": "Updated description",
  "price": 1399.99,
  "category": "Electronics",
  "stockQuantity": 10
}
```

#### Delete Product
```http
DELETE https://localhost:7xxx/api/products/1
```

### 2. Customers API Endpoints

#### Get All Customers
```http
GET https://localhost:7xxx/api/customers
```

#### Get Customer by ID
```http
GET https://localhost:7xxx/api/customers/1
```

#### Get Customer by Email
```http
GET https://localhost:7xxx/api/customers/email/john.doe@email.com
```

#### Get Customers by Country
```http
GET https://localhost:7xxx/api/customers/country/USA
```

#### Search Customers
```http
GET https://localhost:7xxx/api/customers/search?searchTerm=john
```

#### Validate Email
```http
GET https://localhost:7xxx/api/customers/validate-email?email=test@example.com
```

#### Create New Customer
```http
POST https://localhost:7xxx/api/customers
Content-Type: application/json

{
  "firstName": "Test",
  "lastName": "User",
  "email": "test.user@example.com",
  "phoneNumber": "+1234567899",
  "address": "123 Test St",
  "city": "Test City",
  "country": "USA"
}
```

#### Update Customer
```http
PUT https://localhost:7xxx/api/customers/1
Content-Type: application/json

{
  "firstName": "John Updated",
  "lastName": "Doe",
  "email": "john.doe.updated@email.com",
  "phoneNumber": "+1234567890",
  "address": "123 Main St Updated",
  "city": "New York",
  "country": "USA"
}
```

#### Delete Customer
```http
DELETE https://localhost:7xxx/api/customers/1
```

## Testing with cURL

### Products Examples
```bash
# Get all products
curl -X GET "https://localhost:7xxx/api/products" -H "accept: application/json"

# Create a product
curl -X POST "https://localhost:7xxx/api/products" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Test Product",
    "description": "A test product",
    "price": 99.99,
    "category": "Test",
    "stockQuantity": 50
  }'

# Update a product
curl -X PUT "https://localhost:7xxx/api/products/1" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Updated Product",
    "description": "An updated product",
    "price": 149.99,
    "category": "Test",
    "stockQuantity": 25
  }'
```

### Customers Examples
```bash
# Get all customers
curl -X GET "https://localhost:7xxx/api/customers" -H "accept: application/json"

# Create a customer
curl -X POST "https://localhost:7xxx/api/customers" \
  -H "accept: application/json" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Jane",
    "lastName": "Smith",
    "email": "jane.smith@test.com",
    "phoneNumber": "+1987654321",
    "address": "456 Test Ave",
    "city": "Test Town",
    "country": "Canada"
  }'
```

## Testing Error Scenarios

### Validation Errors
```bash
# Try to create a product with missing required fields
curl -X POST "https://localhost:7xxx/api/products" \
  -H "Content-Type: application/json" \
  -d '{"name": ""}'
```

### Not Found Errors
```bash
# Try to get a non-existent product
curl -X GET "https://localhost:7xxx/api/products/999"

# Try to get a non-existent customer
curl -X GET "https://localhost:7xxx/api/customers/999"
```

### Email Validation
```bash
# Try to create a customer with duplicate email
curl -X POST "https://localhost:7xxx/api/customers" \
  -H "Content-Type: application/json" \
  -d '{
    "firstName": "Duplicate",
    "lastName": "User",
    "email": "john.doe@email.com",
    "phoneNumber": "+1111111111",
    "address": "Test Address",
    "city": "Test City",
    "country": "USA"
  }'
```

## PowerShell Testing Script

Save this as `Test-MVCPOC.ps1`:

```powershell
# MVC POC Testing Script
$baseUrl = "https://localhost:7056"  # Update with your actual port

Write-Host "Testing MVC POC API..." -ForegroundColor Green

# Test Products API
Write-Host "`nTesting Products API:" -ForegroundColor Yellow

# Get all products
Write-Host "1. Getting all products..." -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "$baseUrl/api/products" -Method GET
Write-Host "Response: $($response.data.Count) products found" -ForegroundColor Green

# Get product by ID
Write-Host "2. Getting product by ID 1..." -ForegroundColor Cyan
try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/products/1" -Method GET
    Write-Host "Response: Found product '$($response.data.name)'" -ForegroundColor Green
} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

# Search products
Write-Host "3. Searching for 'laptop'..." -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "$baseUrl/api/products/search?searchTerm=laptop" -Method GET
Write-Host "Response: $($response.data.Count) products found" -ForegroundColor Green

# Test Customers API
Write-Host "`nTesting Customers API:" -ForegroundColor Yellow

# Get all customers
Write-Host "1. Getting all customers..." -ForegroundColor Cyan
$response = Invoke-RestMethod -Uri "$baseUrl/api/customers" -Method GET
Write-Host "Response: $($response.data.Count) customers found" -ForegroundColor Green

# Create new customer
Write-Host "2. Creating new customer..." -ForegroundColor Cyan
$newCustomer = @{
    firstName = "Test"
    lastName = "User"
    email = "test.user.$(Get-Random)@example.com"
    phoneNumber = "+1234567890"
    address = "123 Test Street"
    city = "Test City"
    country = "USA"
} | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri "$baseUrl/api/customers" -Method POST -Body $newCustomer -ContentType "application/json"
    Write-Host "Response: Customer created with ID $($response.data.id)" -ForegroundColor Green
    $createdId = $response.data.id

    # Update the created customer
    Write-Host "3. Updating created customer..." -ForegroundColor Cyan
    $updateCustomer = @{
        firstName = "Updated Test"
        lastName = "User"
        email = $response.data.email
        phoneNumber = "+1234567891"
        address = "456 Updated Street"
        city = "Updated City"
        country = "Canada"
    } | ConvertTo-Json

    $response = Invoke-RestMethod -Uri "$baseUrl/api/customers/$createdId" -Method PUT -Body $updateCustomer -ContentType "application/json"
    Write-Host "Response: Customer updated successfully" -ForegroundColor Green

    # Delete the created customer
    Write-Host "4. Deleting created customer..." -ForegroundColor Cyan
    $response = Invoke-RestMethod -Uri "$baseUrl/api/customers/$createdId" -Method DELETE
    Write-Host "Response: Customer deleted successfully" -ForegroundColor Green

} catch {
    Write-Host "Error: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`nTesting completed!" -ForegroundColor Green
```

## Load Testing with k6

Create `load-test.js`:

```javascript
import http from 'k6/http';
import { check, sleep } from 'k6';

export let options = {
  stages: [
    { duration: '2m', target: 100 }, // ramp up
    { duration: '5m', target: 100 }, // stay at 100 users
    { duration: '2m', target: 0 },   // ramp down
  ],
};

const BASE_URL = 'https://localhost:7056'; // Update with your port

export default function () {
  // Test GET products
  let response = http.get(`${BASE_URL}/api/products`);
  check(response, {
    'status is 200': (r) => r.status === 200,
    'response time < 500ms': (r) => r.timings.duration < 500,
  });

  sleep(1);

  // Test GET customers
  response = http.get(`${BASE_URL}/api/customers`);
  check(response, {
    'status is 200': (r) => r.status === 200,
    'response time < 500ms': (r) => r.timings.duration < 500,
  });

  sleep(1);
}
```

Run with: `k6 run load-test.js`

## Expected Performance Metrics

### Response Times (Development Environment)
- **GET requests**: 50-200ms
- **POST requests**: 100-300ms
- **PUT requests**: 100-300ms
- **DELETE requests**: 50-150ms

### Throughput Expectations
- **Concurrent Users**: 100-500 users
- **Requests/Second**: 500-2000 RPS
- **Memory Usage**: 50-200MB
- **CPU Usage**: 10-50%

## Monitoring and Observability

### Console Logs
The application logs detailed request/response information to the console:
- Request details (method, path, headers, body)
- Response details (status code, content type, duration)
- Error information with stack traces

### Custom Middleware Features
1. **Request Logging**: Every API request is logged with unique ID
2. **Error Handling**: Consistent error responses across all endpoints
3. **Performance Tracking**: Request duration measurement
4. **Structured Logging**: JSON-formatted logs for easy parsing

## Comparison Points for Minimal API

When testing against the Minimal API POC, compare:

1. **Response Times**: MVC vs Minimal API latency
2. **Memory Usage**: Runtime memory consumption
3. **Startup Time**: Application bootstrap performance
4. **Throughput**: Requests per second under load
5. **Code Complexity**: Lines of code and maintainability
6. **Developer Experience**: Ease of adding new endpoints

## Troubleshooting

### Common Issues
1. **Port Conflicts**: Check console for actual port number
2. **HTTPS Certificate**: Accept development certificate if prompted
3. **CORS Issues**: API allows all origins for testing
4. **Validation Errors**: Check request body format and required fields

### Useful Commands
```bash
# Check if application is running
netstat -an | findstr :7056

# View application logs
# Check console output where dotnet run was executed

# Stop the application
# Press Ctrl+C in the terminal where it's running
```

---

This comprehensive testing approach will give you detailed insights into the MVC POC performance and functionality to compare against your coworker's Minimal API implementation.
