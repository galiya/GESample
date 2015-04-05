library(shiny)
library(wordcloud)
library(rCharts)
options(RCHART_WIDTH = 800)

tweets.dtm <- getTermMatrix()


shinyServer(function(input, output) {
    
    wordcloud_rep <- repeatable(wordcloud)
    output$chart1 <- renderPlot({
        word.freq <- sort(rowSums(as.matrix(tweets.dtm)))
        wordcloud_rep(words = names(word.freq), freq = word.freq, min.freq = 3, rot.per=0.2, colors=brewer.pal(6, "Dark2"), title="Most Frequent Terms")
    })
    
    output$chart2 <- renderChart({
        assocsTerm <- input$assocsTerm
        corLimit <- input$corLimit
        
        termAssocs <- findAssocs(tweets.dtm, assocsTerm, corLimit)
        termAssocs.freq <- sort(rowSums(termAssocs), decreasing = TRUE)
        termAssocs.df <- data.frame(word = names(termAssocs.freq), freq=termAssocs.freq)
        
        chartTitle <- paste("Associations with term '",assocsTerm,"'")
        p1 <- rPlot(x = list(var = "word", sort = "freq"), y = "freq", data=termAssocs.df, type = 'bar')
        p1$guides(x = list(title = ""))
        p1$guides(y = list(title = "Frequency", max = 1.1))
        p1$addParams(width = 600, height = 300, dom = 'chart2', title = chartTitle)
        return(p1)
    })
})



