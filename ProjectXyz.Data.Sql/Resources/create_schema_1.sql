CREATE TABLE [DisplayLanguages] (
  [Id] GUID NOT NULL ON CONFLICT FAIL CONSTRAINT [UNIQUE_Id] UNIQUE ON CONFLICT FAIL, 
  [Name] VARCHAR(256) NOT NULL ON CONFLICT FAIL);


CREATE TABLE [DisplayStrings] (
  [Id] GUID NOT NULL ON CONFLICT FAIL CONSTRAINT [UNIQUE_Id] UNIQUE ON CONFLICT FAIL, 
  [LanguageId] GUID NOT NULL ON CONFLICT FAIL CONSTRAINT [FK_LanguageId] REFERENCES [DisplayLanguages]([Id]), 
  [Value] TEXT NOT NULL ON CONFLICT FAIL);


CREATE TABLE [EnchantmentCalculations] (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [Name] VARCHAR(64) NOT NULL ON CONFLICT FAIL COLLATE NOCASE);


CREATE TABLE [Stats] (
  [Id] GUID NOT NULL ON CONFLICT FAIL CONSTRAINT [UNIQUE_Id] UNIQUE ON CONFLICT FAIL, 
  [Name] VARCHAR(256) NOT NULL ON CONFLICT FAIL COLLATE NOCASE, 
  [DisplayStringId] GUID NOT NULL ON CONFLICT FAIL CONSTRAINT [FK_DisplayStringId] REFERENCES [DisplayStrings]([Id]) COLLATE NOCASE);


CREATE TABLE [Enchantments] (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [StatId] GUID NOT NULL ON CONFLICT FAIL CONSTRAINT [FK_StatId] REFERENCES [Stats]([Id]), 
  [CalculationId] GUID NOT NULL ON CONFLICT FAIL CONSTRAINT [FK_CalculationId] REFERENCES [EnchantmentCalculations]([Id]), 
  [TriggerId] GUID NOT NULL ON CONFLICT FAIL, 
  [StatusTypeId] GUID NOT NULL ON CONFLICT FAIL, 
  [MinimumValue] FLOAT NOT NULL ON CONFLICT FAIL, 
  [MaximumValue] FLOAT NOT NULL ON CONFLICT FAIL,
  [MinimumDuration] FLOAT NOT NULL ON CONFLICT FAIL, 
  [MaximumDuration] FLOAT NOT NULL ON CONFLICT FAIL);


CREATE TABLE [EnchantmentStatuses] (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [Name] VARCHAR(64) NOT NULL ON CONFLICT FAIL COLLATE NOCASE);


CREATE TABLE [EnchantmentTriggers] (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [Name] VARCHAR(64) NOT NULL ON CONFLICT FAIL COLLATE NOCASE);

CREATE TABLE [ItemAffixes] (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [Name] VARCHAR(128) NOT NULL ON CONFLICT FAIL COLLATE NOCASE,
  [IsPrefix] BIT NOT NULL ON CONFLICT FAIL,
  [MinimumLevel] INT NOT NULL ON CONFLICT FAIL,
  [MaximumLevel] INT NOT NULL ON CONFLICT FAIL,
  [AffixMagicTypesId] BIT NOT NULL ON CONFLICT FAIL,
  [AffixEnchantmentsId] GUID NOT NULL ON CONFLICT FAIL);

CREATE TABLE AffixEnchantments (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [EnchantmentId] GUID NOT NULL ON CONFLICT FAIL);
  
CREATE TABLE AffixMagicTypes (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [MagicTypeId] GUID NOT NULL ON CONFLICT FAIL);

CREATE TABLE MagicTypes (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [Name] VARCHAR(64) NOT NULL ON CONFLICT FAIL COLLATE NOCASE);

CREATE TABLE MagicTypesRandomAffixes (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [MagicTypeId] GUID NOT NULL ON CONFLICT FAIL,
  [MinimumAffixes] GUID NOT NULL ON CONFLICT FAIL,
  [MaximumAffixes] GUID NOT NULL ON CONFLICT FAIL);

CREATE TABLE Diseases (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [Name] VARCHAR(64) NOT NULL ON CONFLICT FAIL COLLATE NOCASE,
  [DiseaseStatesId] GUID NOT NULL ON CONFLICT FAIL);

CREATE TABLE DiseaseStates (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [Name] VARCHAR(64) NOT NULL ON CONFLICT FAIL COLLATE NOCASE,
  [PreviousStateId] GUID NOT NULL ON CONFLICT FAIL, 
  [NextStateId] GUID NOT NULL ON CONFLICT FAIL, 
  [DiseaseStatesEnchantmentsId] GUID NOT NULL ON CONFLICT FAIL,
  [DiseaseSpreadTypeId] GUID NOT NULL ON CONFLICT FAIL);

CREATE TABLE DiseaseStatesEnchantments (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [DiseaseStateId] GUID NOT NULL ON CONFLICT FAIL,
  [EnchantmentId] GUID NOT NULL ON CONFLICT FAIL);

CREATE TABLE DiseaseSpreadTypes (
  [Id] GUID NOT NULL ON CONFLICT FAIL, 
  [Name] VARCHAR(64) NOT NULL ON CONFLICT FAIL COLLATE NOCASE);