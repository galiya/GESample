#install.packages("jsonlite")
#install.packages("tm")

#install.packages("memoise")
#install.packages("wordcloud")

#library(jsonlite)
library(tm)
#library(memoise)
#library(rCharts)
#library(wordcloud)
#setwd("~/Projects/GESample/GESample/")

#jsonfile <- "data/Tweets.txt"
#data <- fromJSON(sprintf("[%s]", paste(readLines(jsonfile), collapse=",")))
#tweetsText <- as.data.frame(data$text)

#select top 200 TweetText from Tweets

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

tweets.dtm <- getTermMatrix()
#dim(tweets.dtm)

#Display a wordcloud for terms
set.seed(100) #to ensure the wordcloud has the same layout 
word.freq <- sort(rowSums(as.matrix(tweets.dtm), decreasing = TRUE)
wordcloud(words = names(word.freq), freq = word.freq, min.freq = 3, rot.per=0.2, colors=brewer.pal(6, "Dark2"))


#Display a bar chart for term associatons
assocsTerm <- 'snp'
corLimit <- 0.2
termAssocs <- findAssocs(tweets.dtm, assocsTerm, corLimit)

termAssocs.freq <- sort(rowSums(termAssocs), decreasing = TRUE)
termAssocs.df <- data.frame(word = names(termAssocs.freq), freq=termAssocs.freq)

require(rCharts)
chartTitle <- paste("Associations with term '",assocsTerm,"'")
p1 <- rPlot(x = list(var = "word", sort = "freq"), y = "freq", data=termAssocs.df, type = 'bar')
p1$guides(x = list(title = ""))
p1$guides(y = list(title = "Frequency", max = 1.1))
p1$addParams(width = 600, height = 300, dom = 'chart2', title = chartTitle)
p1.print()
