CREATE TABLE [dbo].[RefreshToken](
[token_id] INT    IDENTITY(1,1) NOT NULL,
[user_id] INT     NOT NULL,
[token] VARCHAR(200)  NOT NULL,
[expire_date]   DATETIME NOT NULL



    CONSTRAINT[PK_RefreshToken] PRIMARY KEY CLUSTERED ([token_id] ASC),
    FOREIGN KEY ([user_id]) REFERENCES [dbo].[User] ([user_id]) ON DELETE CASCADE ON UPDATE CASCADE,
);