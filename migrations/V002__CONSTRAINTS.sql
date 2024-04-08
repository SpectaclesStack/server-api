-- Adding unique constraint on Email column in Users table
ALTER TABLE Users
ADD CONSTRAINT UniqueEmail UNIQUE (Email);

-- Adding unique constraint on UserName column in Users table
ALTER TABLE Users
ADD CONSTRAINT UniqueUserName UNIQUE (UserName);

-- Adding foreign key constraint on UserId column in Questions table
ALTER TABLE Questions
ADD CONSTRAINT QuestionsFK FOREIGN KEY (UserId) REFERENCES Users(UserId);


-- Adding foreign key constraint on UserId column in Answers table
ALTER TABLE Answers
ADD CONSTRAINT AnswersFK1 FOREIGN KEY (UserId) REFERENCES Users(UserId);

-- Adding foreign key constraint on QuestionId column in Answers table
ALTER TABLE Answers
ADD CONSTRAINT AnswersFK2 FOREIGN KEY (QuestionId) REFERENCES Questions(QuestionId);
