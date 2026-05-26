*COIN PARKNG SYSTEM*

A robust Parking Management System built with C# and WPF, utilizing the MVVM (Model-View-ViewModel) architectural pattern. Features

Entry System: Automatically records the vehicle entry time.
Exit System:Calculates parking fees automatically upon vehicle exit.
Clean Architecture: Implements the MVVM pattern to ensure a clear separation of concerns, resulting in maintainable and scalable code.
Project Structure

Models/: Contains data structures (e.g., vehicle information) and core business logic.
ViewModels/: Acts as the mediator between the UI and the Model, handling user interactions and state logic.
Views/: Contains the user interface layouts (XAML) for Entry and Exit screens.
Services/: Manages data operations and backend tasks, keeping the logic separated from the UI.
Technologies Used

Language: C#
Framework: .NET (WPF)
Design Pattern: MVVM
Key Concept: Polymorphism (Applied in fee calculation strategies)
How to Use

Clone this repository to your local machine.
Open the solution file (.sln) in Visual Studio.
Build the project and run the application.
