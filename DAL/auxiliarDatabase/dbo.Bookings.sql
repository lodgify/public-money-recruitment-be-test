CREATE TABLE [dbo].[Bookings]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [RentalId] INT NULL, 
    [Start] DATETIME NULL, 
    [Nights] INT NULL, 
    [Unit] INT NULL, 
    CONSTRAINT [FK_Bookings_ToTable] FOREIGN KEY ([RentalId]) REFERENCES [Rentals]([Id]) 
)
