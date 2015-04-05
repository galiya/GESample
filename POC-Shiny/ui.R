library(shiny)
library(rCharts)
options(RCHART_LIB = 'polycharts')

#library(shinyIncubator)


assocsChoices <- c('labour', 'snp', 'ukip')

shinyUI(fluidPage(
    titlePanel("GE Sample"),
    sidebarLayout(
         position="right",
         sidebarPanel(
             conditionalPanel(condition = "input.tabs == 'Term associations'",
                selectInput(inputId = "assocsTerm",
                        label = "Select a term to show associations",
                        choices = assocsChoices,
                        selected = "labour")
                ),
             conditionalPanel(condition = "input.tabs == 'Term associations'",
                sliderInput("corLimit", label = h3("Correlation limit"),
                         min = 0, max = 1, value = 0.2)
             )
         ),
    
        mainPanel(
            tabsetPanel(id="tabs",
                tabPanel("Term associations", showOutput("chart2", "polycharts")),
                tabPanel("Wordcloud", plotOutput("chart1"))
            )
        )
    )
))
