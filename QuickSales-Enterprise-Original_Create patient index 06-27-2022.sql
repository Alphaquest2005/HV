

GO
PRINT N'Creating Index [dbo].[Persons].[IX_Persons]...';


GO
CREATE NONCLUSTERED INDEX [IX_Persons]
    ON [dbo].[Persons]([Id] ASC, [FirstName] ASC, [LastName] ASC, [PhoneNumber] ASC);


GO
PRINT N'Creating Index [dbo].[Persons_Patient].[IX_Persons_Patient]...';


GO
CREATE NONCLUSTERED INDEX [IX_Persons_Patient]
    ON [dbo].[Persons_Patient]([Id] ASC, [CardId] ASC);


GO
PRINT N'Update complete.';


GO
