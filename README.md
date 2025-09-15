# Customer/Lead Image Management API

A comprehensive .NET 8 Web API for managing customer and lead profile images with intelligent business rules and clean architecture implementation.

## Project Overview

This API implements a robust image management system that allows users to upload, view, and delete images for customer and lead profiles. The system enforces a maximum of 10 images per customer/lead with images stored as Base64-encoded strings in the database.

## Key Features

### Smart Image Limit Enforcement

- Maximum 10 images per customer/lead profile
- Intelligent validation prevents uploading the 11th image
- Graceful error handling with clear feedback messages

### Advanced Image Handling

- Base64 storage for direct database persistence
- Multiple format support (JPEG, PNG, GIF, WebP)
- File size validation (5MB maximum per image)
- Content type detection from data URLs

### Clean Architecture Implementation

- Domain Layer: Pure business entities with validation rules
- Application Layer: CQRS pattern with MediatR
- Persistence Layer: Repository pattern with Entity Framework Core
- API Layer: RESTful endpoints with comprehensive documentation

## Getting Started

### Prerequisites

- .NET 8.0 SDK or later
- PostgreSQL (configured in appsettings.json)
- Visual Studio 2022 or VS Code

### Installation & Setup

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd profile-image-management/ImageManagement
   ```

2. **Update database connection**

   - Edit `ImageManagement.API/appsettings.json`
   - Configure your PostgreSQL connection string:

   ```json
   {
     "ConnectionStrings": {
       "ImageManagementConnectionString": "User ID=postgres;Password=your_password;Server=localhost;Port=5432;Database=ImageManagementDB;Integrated Security=true;Pooling=true;"
     }
   }
   ```

3. **Build the project**

   ```bash
   dotnet build ImageManagement.API/ImageManagement.API.csproj
   ```

4. **Run the API**

   ```bash
   dotnet run --project ImageManagement.API/ImageManagement.API.csproj
   ```

5. **Access Swagger Documentation**
   - Navigate to: `https://localhost:7102/swagger` or `http://localhost:5299/swagger`

## API Endpoints

### Images Management (/api/images)

| Method | Endpoint | Description               | Parameters                              |
| ------ | -------- | ------------------------- | --------------------------------------- |
| POST   | /upload  | Upload multiple images    | customerId or leadId + images array     |
| GET    | /        | Get all images for entity | customerId or leadId (query)            |
| DELETE | /{id}    | Delete specific image     | id (path), customerId or leadId (query) |

## Usage Examples

### Upload Images to Customer

```json
POST /api/images/upload
{
  "customerId": 1,
  "images": [
    {
      "base64Data": "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQEAYABgAAD...",
      "fileName": "profile-photo.jpg",
      "description": "Main profile photo",
      "isMainImage": true
    }
  ]
}
```

### Get All Images

```bash
GET /api/images?customerId=1
```

### Delete Image

```bash
DELETE /api/images/123?customerId=1
```

## Architecture & Design Patterns

### Domain Models

- **Customer**: Name, email, phone, address + image collection
- **Lead**: Name, email, phone, company, source, contact date + image collection
- **ProfileImage**: Base64 data, metadata, main image flag, ownership validation

### Key Design Patterns

- **CQRS**: Command Query Responsibility Segregation with MediatR
- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management
- **Clean Architecture**: Dependency inversion and separation of concerns

### Business Rules Implementation

```csharp
// Domain level validation
public bool CanAddMoreImages => Images?.Count < 10;
public int RemainingImageSlots => Math.Max(0, 10 - (Images?.Count ?? 0));

// Repository level enforcement
public async Task<bool> CanAddMoreImagesAsync(int? customerId, int? leadId)
{
    var count = customerId.HasValue
        ? await CountByCustomerAsync(customerId.Value)
        : await CountByLeadAsync(leadId.Value);
    return count < 10;
}
```

## Technology Stack

### Core Framework

- **.NET 8.0**: Latest LTS version with improved performance
- **ASP.NET Core Web API**: RESTful API framework

### Data & Persistence

- **Entity Framework Core 8.0**: ORM with Code First approach
- **PostgreSQL**: Robust relational database
- **Npgsql**: PostgreSQL provider for .NET

### Architecture & Patterns

- **MediatR 12.4**: CQRS implementation and request handling
- **AutoMapper 12.0**: Object-to-object mapping
- **FluentValidation 11.9**: Model validation framework

### Documentation & Testing

- **Swagger/OpenAPI**: Interactive API documentation
- **XML Documentation**: Enhanced API descriptions

## Configuration

### Database Connection

```json
{
  "ConnectionStrings": {
    "ImageManagementConnectionString": "User ID=postgres;Password=your_password;Server=localhost;Port=5432;Database=ImageManagementDB;Integrated Security=true;Pooling=true;"
  }
}
```

### API Settings

- **HTTPS**: `https://localhost:7102`
- **HTTP**: `http://localhost:5299`
- **Swagger UI**: `/swagger`

## Error Handling & Validation

### Image Upload Validation

- Format validation: Base64 format checking
- Size limits: 5MB maximum per image
- Count limits: Maximum 10 images per entity
- Content type: JPEG, PNG, GIF, WebP support
- Ownership: Entity existence validation

### Error Response Format

```json
{
  "success": false,
  "message": "Maximum of 10 images allowed. Currently have 10 images.",
  "errors": ["Image 1: File size exceeds maximum allowed size of 5MB."]
}
```

## Implementation Highlights

### 1. Smart 10-Image Limit Enforcement

```csharp
// Multi-layer validation approach
var availableSlots = MaxImagesPerEntity - currentImageCount;
if (availableSlots <= 0)
{
    return BadRequest($"Maximum of {MaxImagesPerEntity} images allowed. Currently have {currentImageCount} images.");
}

// Process only available slots
foreach (var imageDto in request.Images.Take(availableSlots))
```

### 2. Automatic Content Type Detection

```csharp
if (base64Data.StartsWith("data:"))
{
    var match = Regex.Match(base64Data, @"^data:([^;]+);base64,(.+)$");
    if (match.Success)
    {
        contentType = match.Groups[1].Value;
        base64Data = match.Groups[2].Value;
    }
}
```

## Future Enhancements

### Potential Improvements

- **Image Compression**: Automatic resizing and optimization
- **Cloud Storage**: Azure Blob Storage or AWS S3 integration
- **Image Processing**: Thumbnail generation, format conversion
- **Caching**: Redis for frequently accessed images
- **Audit Trail**: Track image upload/deletion history
- **Batch Operations**: Bulk image management

### Performance Optimizations

- **Lazy Loading**: On-demand image loading
- **Pagination**: Large image collection handling
- **Background Processing**: Async image processing
- **CDN Integration**: Global image delivery

## License

This project is licensed under the MIT License - see the LICENSE file for details.
---

**Built with clean architecture principles and modern .NET 8 practices**
