# Code360.Assessment
Assessment Exam for Code360(EastVantage)

Important points for this solution:

1. This solution implements the Dependency injection pattern to make it testable.
2. This implements Behavior-Driven Development(BDD) using the NBehave.Spec.NUnit and Moq packages.
3. This uses NUnit for Unit Testing.
4. Property injection is implemented in the Repositories.
5. Constructor Injection is implemented in the Controller classes.
6. Test project in Code360.Assessment.Web.Tests.
7. Uses the ImageResizer plugin to appropriately resize the images in the frontend.
8. Classes Created:
  8.1. /Controllers/ControllerBase.cs- Abstract Class inherited by the the Controller classes that contains common Constructor, property, and method implementations.
  8.2. /Models/UserBase- Abstract class implemented by user profile entities.
  8.3. /Helpers/ImageHelper- Helper class used for image processing and manipulation.
  8.4. /Helpers/ValidationHelper- Helper class that handles custom validation.
