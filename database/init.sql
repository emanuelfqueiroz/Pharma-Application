Create Database PharmaRep;
Go
Use PharmaRep

Go

CREATE TABLE [PharmaUser]
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    FullName VARCHAR(50) Not NULL,
    Email VARCHAR(50) NOT NULL,
    EncodedPassword VARCHAR(128) NOT NULL
)

CREATE UNIQUE INDEX IDX_User_Email ON [PharmaUser] (Email)

Go
Insert into PharmaUser (FullName, Email, EncodedPassword) Values('Admin', 'admin@example','****')

GO
CREATE TABLE [MedicalCondition]
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    [Name] VARCHAR(50) NOT NULL,
    Description VARCHAR(500) NOT NULL
)
CREATE UNIQUE INDEX IDX_MedicalCondition_Name ON [MedicalCondition] ([Name])

Insert into MedicalCondition ([Name], Description) Values ('Hypertension', 'High blood pressure')
Insert into MedicalCondition ([Name], Description) Values ('High Cholesterol', 'High cholesterol')
Insert into MedicalCondition ([Name], Description) Values ('Diabetes', 'Diabetes')
Insert into MedicalCondition ([Name], Description) Values ('Common Cold', 'Common Cold')
Insert into MedicalCondition ([Name], Description) Values ('Influenza', 'Influenza')
Insert into MedicalCondition ([Name], Description) Values ('Allergies', 'Allergies')
Insert into MedicalCondition ([Name], Description) Values ('Coronary Artery Disease', 'Coronary Artery Disease')
Insert into MedicalCondition ([Name], Description) Values ('Asthma', 'Asthma')
Insert into MedicalCondition ([Name], Description) Values ('COPD', 'COPD')
Insert into MedicalCondition ([Name], Description) Values ('Stroke', 'Stroke')
Insert into MedicalCondition ([Name], Description) Values ('Metabolic Syndrome', 'Metabolic Syndrome')
Insert into MedicalCondition ([Name], Description) Values ('Obesity', 'Obesity')
Insert into MedicalCondition ([Name], Description) Values ('Osteoarthritis', 'Osteoarthritis')
Insert into MedicalCondition ([Name], Description) Values ('Rheumatoid Arthritis', 'Rheumatoid Arthritis')
Insert into MedicalCondition ([Name], Description) Values ('Osteoporosis', 'Osteoporosis')
Insert into MedicalCondition ([Name], Description) Values ('Cancer', 'Cancer')
Insert into MedicalCondition ([Name], Description) Values ('Alzheimer''s Disease', 'Alzheimer''s Disease')
Insert into MedicalCondition ([Name], Description) Values ('Dementia', 'Dementia')
Insert into MedicalCondition ([Name], Description) Values ('Parkinson''s Disease', 'Parkinson''s Disease')
Insert into MedicalCondition ([Name], Description) Values ('Epilepsy', 'Epilepsy')
Insert into MedicalCondition ([Name], Description) Values ('Liver Disease', 'Liver Disease')
Insert into MedicalCondition ([Name], Description) Values ('Kidney Disease', 'Kidney Disease')
Insert into MedicalCondition ([Name], Description) Values ('Peptic Ulcer Disease', 'Peptic Ulcer Disease')
Insert into MedicalCondition ([Name], Description) Values ('HIV/AIDS', 'HIV/AIDS')
Insert into MedicalCondition ([Name], Description) Values ('Depression', 'Depression')
Insert into MedicalCondition ([Name], Description) Values ('Anxiety Disorders', 'Anxiety Disorders')
Insert into MedicalCondition ([Name], Description) Values ('Bipolar Disorder', 'Bipolar Disorder')
Insert into MedicalCondition ([Name], Description) Values ('Schizophrenia', 'Schizophrenia')
Insert into MedicalCondition ([Name], Description) Values ('Autism', 'Autism')
Insert into MedicalCondition ([Name], Description) Values ('ADHD (Attention deficit hyperactivity disorder)', 'Attention deficit hyperactivity disorder')

Go
CREATE TABLE [MedicalReaction]
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    [Reaction] VARCHAR(50) NOT NULL,
    Description VARCHAR(500)  NULL
)
CREATE UNIQUE INDEX IDX_MedicalReaction_Name ON [MedicalReaction] ([Reaction])
Go
INSERT INTO MedicalReaction (Reaction) VALUES
('Rash'),
('Nausea'),
('Headache'),
('Dizziness'),
('Fatigue'),
('Fever'),
('Diarrhea'),
('Cough'),
('Shortness of breath'),
('Vomiting'),
('Abdominal pain'),
('Constipation'),
('Itching'),
('Dry mouth'),
('Weight gain'),
('Weight loss'),
('Muscle pain'),
('Joint pain'),
('Sweating'),
('Increased heart rate'),
('Decreased heart rate'),
('High blood pressure'),
('Low blood pressure'),
('Insomnia'),
('Excessive sleepiness'),
('Loss of appetite'),
('Increased appetite'),
('Swelling'),
('Bloating')

Go
Create Table Brand
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    BrandName VARCHAR(255) NOT NULL,
    Description VARCHAR(255) NULL,
    WebSite VARCHAR(255) NULL,
)
CREATE UNIQUE INDEX IDX_Brand_Name ON [Brand] ([BrandName])
GO
Insert INto Brand (BrandName, Description, WebSite) VALUES
('Brand Sample A','Description Brand','https://brandwebsite.brandA'),
('Brand Sample B','Description Brand','https://brandwebsite.brandB'),
('Brand Sample C','Description Brand','https://brandwebsite.brandC'),
('Brand Sample D','Description Brand','https://brandwebsite.brandD'),
('Brand Sample E','Description Brand','https://brandwebsite.brandE')

Go

CREATE TABLE Drug
(
    Id INT PRIMARY KEY IDENTITY(1,1),
    BrandId INT NOT NULL,
    [Name] VARCHAR(255) NOT NULL,
    Description VARCHAR(MAX) NOT NULL,
    UserCreatedId INT NOT NULL,
    DateCreated DATETIME NOT NULL,
    DrugStatus INT NOT NULL,
    WebSite VARCHAR(255) NULL,
    CONSTRAINT FK_Drug_UserCreated FOREIGN KEY (UserCreatedId) REFERENCES PharmaUser(Id),
    CONSTRAINT FK_Drug_Brand FOREIGN KEY (BrandId) REFERENCES Brand(Id)
)

CREATE TABLE DrugReaction
(
    DrugId INT NOT NULL,
    ReactionId INT NOT NULL,
    PRIMARY KEY (DrugId, ReactionId),
    FOREIGN KEY (DrugId) REFERENCES Drug(Id),
    FOREIGN KEY (ReactionId) REFERENCES MedicalReaction(Id)
)
CREATE TABLE DrugIndication
(
    DrugId INT NOT NULL,
    IndicationId INT NOT NULL,
    PRIMARY KEY (DrugId, IndicationId),
    FOREIGN KEY (DrugId) REFERENCES Drug(Id),
    FOREIGN KEY (IndicationId) REFERENCES MedicalCondition(Id)
)

Go

INSERT INTO Drug (BrandId, [Name], Description, UserCreatedId, DateCreated, DrugStatus, WebSite) VALUES
(1, 'Zestril', 'Lisinopril is used to treat high blood pressure. Lowering high blood pressure helps prevent strokes, heart attacks, and kidney problems. It is also used to treat heart failure and to improve survival after a heart attack.', 1, GETDATE(), 1, 'https://medicinewebsite.drugA.com'),
(2, 'Atorvastatin', 'it is used to treat high blood pressure. Lowering high blood pressure helps prevent strokes, heart attacks, and kidney problems. It is also used to treat heart failure and to improve survival after a heart attack.', 1, GETDATE(), 1, 'https://medicinewebsite.drugB.com'),
(3, 'Levothyroxine', 'Description Drug C', 1, GETDATE(), 1, 'https://medicinewebsite.drugC.com'),
(1, 'Amlodipine', 'Description Drug D', 1, GETDATE(), 1, 'https://medicinewebsite.drugD.com'),
(2, 'Omeprazole', 'Description Drug E', 1, GETDATE(), 1, 'https://medicinewebsite.drugE.com'),

(1, 'Metformin', 'Description Drug ', 1, GETDATE(), 1, 'https://medicinewebsite.drugD.com'),
(2, 'Metoprolol', 'Description Drug ', 1, GETDATE(), 1, 'https://medicinewebsite.drugE.com'),
(1, 'Simvastatin', 'Description Drug ', 1, GETDATE(), 1, 'https://medicinewebsite.drugD.com'),
(2, 'Losartan', 'Description Drug ', 1, GETDATE(), 1, 'https://medicinewebsite.drugE.com'),
(2, 'Albuterol', 'Description Drug ', 1, GETDATE(), 1, 'https://medicinewebsite.drugE.com')

Go

INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (1,2);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (2,3);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (3,4);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (4,5);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (5,6);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (6,7);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (7,8);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (8,9);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (9,10);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (10,1);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (1,3);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (2,4);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (3,5);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (4,6);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (5,7);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (6,8);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (7,9);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (8,10);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (9,1);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (10,2);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (1,4);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (2,5);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (3,6);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (4,7);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (5,8);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (6,9);
INSERT INTO DrugIndication (DrugId, IndicationId) VALUES (7,10);


INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (1,2);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (2,3);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (3,4);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (4,5);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (5,6);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (6,7);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (7,8);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (8,9);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (9,10);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (10,1);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (1,3);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (2,4);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (3,5);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (4,6);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (5,7);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (6,8);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (7,9);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (8,10);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (9,1);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (10,2);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (1,4);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (2,5);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (3,6);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (4,7);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (5,8);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (6,9);
INSERT INTO DrugReaction (DrugId, ReactionId) VALUES (7,10);


Go
Create OR ALTER VIEW DrugAggregateView AS
SELECT Drug.Id, Drug.Name, Drug.Description, Drug.DrugStatus, Drug.DateCreated,
                    Brand.BrandName, Brand.Id as BrandId,
                    U.FullName as UserCreatedName, U.Id as UserCreatedId,
                    (
                            SELECT DrugIndication.IndicationId as Id, MedicalCondition.[Name]
                            FROM DrugIndication
                            Join MedicalCondition on DrugIndication.IndicationId = MedicalCondition.Id
                            WHERE DrugIndication.DrugId = Drug.Id
                            FOR JSON PATH
                    ) AS MedicalConditionData,
                    (
                            SELECT MedicalReaction.Id as Id, MedicalReaction.Reaction as [Name]
                            FROM DrugReaction
                            Join MedicalReaction on DrugReaction.ReactionId = MedicalReaction.Id
                            WHERE DrugReaction.DrugId = Drug.Id
                            FOR JSON PATH
                    ) AS MedicalReactionData
                    FROM Drug 
                    INNER JOIN Brand ON Drug.BrandId = Brand.Id
                    INNER JOIN PharmaUser as U ON U.Id = Drug.UserCreatedId
