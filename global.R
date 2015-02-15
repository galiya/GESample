#install.packages("jsonlite")
#install.packages("tm")
#install.packages("SnowballC")
#install.packages("memoise")

library(jsonlite)
library(tm)
library(SnowballC)
library(memoise)

jsonfile <- "data/Tweets.txt"
data <- fromJSON(sprintf("[%s]", paste(readLines(jsonfile), collapse=",")))
tweetsText <- data$text

getTermMatrix <- memoise(function() {
    
    #remove urls
    tweetsText <- gsub("http.*$", "", gsub('http.*\\s', ' ', tweetsText))
    #remove RTs
    tweetsText <- gsub('(RT|via)((?:\\b\\W*@\\w+)+):', '', tweetsText)
    # remove blank spaces at the beginning
    tweetsText <- gsub("^ ", "", tweetsText)
    # remove blank spaces at the end
    tweetsText <- gsub(" $", "", tweetsText)
    # remove control chars
    tweetsText <- gsub("[[:cntrl:]]", "", tweetsText)
    
    
    corpus <- Corpus(VectorSource(tweetsText), readerControl = list(language = "en"))
    corpus <- tm_map(corpus, content_transformer(tolower))
    #corpus <- tm_map(corpus, removeNumbers)
    corpus <- tm_map(corpus, removePunctuation) #can be modified to preserve # 
    corpus <- tm_map(corpus, stripWhitespace)
    corpus <- tm_map(corpus, removeWords, c(stopwords("english"), "general", "election", "ge15", "ge2015", "2015"))
    
    #tweets.dtm <- 
    TermDocumentMatrix(corpus, control = list(minWordLength = 1))
})