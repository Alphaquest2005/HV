USE [QuickSales-Enterprise]
GO

/****** Object:  Table [dbo].[PrescriptionImage]    Script Date: 8/22/2021 6:56:09 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[PrescriptionImage](
	[TransactionId] [int] NOT NULL,
	[Image] [image] NOT NULL,
	[Path] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_PrescriptionImage] PRIMARY KEY CLUSTERED 
(
	[TransactionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

ALTER TABLE [dbo].[PrescriptionImage]  WITH CHECK ADD  CONSTRAINT [FK_PrescriptionImage_TransactionBase_Prescription] FOREIGN KEY([TransactionId])
REFERENCES [dbo].[TransactionBase_Prescription] ([TransactionId])
ON DELETE CASCADE
GO

ALTER TABLE [dbo].[PrescriptionImage] CHECK CONSTRAINT [FK_PrescriptionImage_TransactionBase_Prescription]
GO


