library(shiny)
library(dplyr)
library(reshape2)
library(purrr)
library(stringr)
library(plotly)
library(tibble)
library(DT)
library(shinythemes)
library(shinydashboard)
library(shinycssloaders)
library(shinyWidgets)
library("ggrepel")
library(caret)


#===========CALCULATING PCA========================
d_mahal1 <- read.csv("C:/Users/Yana/Documents/Hololens_MP/Assets/dimensionality_reduction/data/dist_m.csv")


# d_mahal2 <- reactive({
  all.findings1 <- d_mahal1
  
  # removing variables with unit variance
  all.findings12 <-
     all.findings1[, c(8:ncol(all.findings1))]# [, apply(all.findings1[, c(7:ncol(all.findings1))], 2, var, na.rm = TRUE) != 0]
  
  d_mahal2 <-all.findings12


# d_mahal <- reactive({
  all.findings1 <- d_mahal1
  all.findings12 <- d_mahal2
  
  # principal components
  ### is really important condition???
  if (nrow(all.findings12) > ncol(all.findings12)) {
    stdmat <- scale(all.findings12)
    eig <- eigen(cor(stdmat, use = 'nrow(all.findings12) > ncol(all.findings12)'))
    scores <- stdmat %*% eig$vectors
    
    D <-
      diag(scores %*% MASS::ginv(diag(eig$values)) %*% t(scores))
    
    all.f2 <-
      all.findings1[, c(1:6)] %>% bind_cols(all.findings12) %>%
      mutate(Distance = round(sqrt(D), 3)) %>% mutate(D1 = round(D, 3))
    
    # Function to generate control limits of Mahalanobis Distance from Beta Distribution
    Mahal_CL <- function(p, m, alpha) {
      sqrt(((m - 1) ^ 2) / m * qbeta(1 - alpha, p / 2, (m - p - 1) / 2))
    }
    
    # adding mahalanobis distances
    all.f3 <-
      all.f2 %>% group_by(USUBJID) %>% mutate(Status =
                                                ifelse(
                                                  Distance >= Mahal_CL(ncol(all.f2) - 8, nrow(all.f2), .0015),
                                                  "Severe Outlier",
                                                  ifelse(
                                                    Distance <= Mahal_CL(ncol(all.f2) - 8, nrow(all.f2), .9985),
                                                    "Severe Inlier",
                                                    ifelse(
                                                      Distance >= Mahal_CL(ncol(all.f2) - 8, nrow(all.f2), .025) &
                                                        Distance < Mahal_CL(ncol(all.f2) -
                                                                              8, nrow(all.f2), .0015),
                                                      "Moderate Outlier",
                                                      ifelse(
                                                        Distance <= Mahal_CL(ncol(all.f2) - 8, nrow(all.f2), .975) &
                                                          Distance > Mahal_CL(ncol(all.f2) -
                                                                                8, nrow(all.f2), .9985),
                                                        "Moderate Inlier",
                                                        "Within Range"
                                                      )
                                                    )
                                                  )
                                                )) %>% ungroup()
    
    d_mahal <-all.f3
  } else {
    all.f3 <- NULL
  }
  d_mahal <-all.f3



  
write.csv(d_mahal, "dist_m_new.csv")  
  
all.f3 <- d_mahal

all.findings1 <-
  d_mahal1 

all.f31 <-
  all.findings1[, c(7:ncol(all.findings1))][, apply(all.findings1[, c(7:ncol(all.findings1))], 2, var, na.rm = TRUE) != 0]

# principal component analysis and biplot
stdmat <- scale(all.f31)
eig <- eigen(cor(stdmat, use = 'complete.obs'))
scores <- stdmat %*% eig$vectors

# proportion of variance contribution
prop <- round(eig$values / sum(eig$values) * 100, 2)

# Loadings
loads <- eig$vectors



USUBJID <- all.f3$USUBJID
MDistance <- all.f3$Distance
Status<- all.f3$Status
x1 <-  scores[, as.numeric(1)]
y1 <-  scores[, as.numeric(2)]
z1 <- scores[, as.numeric(3)]
x2 <-  scores[, as.numeric(4)]
y2 <-  scores[, as.numeric(5)]
z2 <- scores[, as.numeric(6)]
x3 <-  scores[, as.numeric(7)]
y3 <-  scores[, as.numeric(8)]
z3 <- scores[, as.numeric(9)]
color <-  all.f3$Distance
for_file <- data.frame(USUBJID,MDistance,Status, x1,y1,z1, x2,y2,z2, x3,y3,z3, color   )

write.csv(for_file, file="C:/Users/Yana/Documents/Hololens_MP/Assets/dimensionality_reduction/data/PCA_short_data.csv", row.names = TRUE,  quote = FALSE)

########### plot subjects



validate(need(!is.null(sel_sub),
              "Select a subject from the Table."))



###to make for 

for (i in 1:nrow(d_mahal))
{
  
z <- d_mahal[i,]
all.f31 <- d_mahal2
stdmat <- scale(all.f31)
eig <- eigen(cor(stdmat, use = 'complete.obs'))
scores <- stdmat %*% eig$vectors

t1 <-
  scores %*% sqrt(MASS::ginv(diag(eig$values))) %*% t(eig$vectors)
t2 <-
  round((t1 * t1 / (
    as.matrix(d_mahal$D1) %*% rep(1, ncol(d_mahal2))
  )) * 100, 2)
colnames(t2) <- colnames(all.f31)
cont <-
  d_mahal %>% select(USUBJID) %>% cbind(t2) %>% filter(USUBJID %in% z$USUBJID)
pcont <-
  melt(cont, id.vars = "USUBJID") %>% group_by(USUBJID) %>%
  mutate(percont = 100 * (value / sum(value))) %>% ungroup()
pcont1 <-
  pcont %>% arrange(USUBJID, desc(percont)) %>%
  group_by(USUBJID) %>% mutate(cump = cumsum(percont)) %>% ungroup() #%>% filter(cump <= 95)

cont_list <- list()

  pcont2 <- head(pcont1,5)
  
  ### cumulative and contribution graph
  image <- ggplot(data=pcont2, aes(x=reorder(variable, -value))) + 
    geom_bar(aes(y=value,fill="Contribution of field"), width=0.9, stat="identity") +
    geom_line(aes(y=cump,group=1,linetype="Cumulative contribution"))+
    geom_point(aes(y=cump))+
    xlab("Variables") + ylab("% contribution") +
    labs(fill="",linetype="")+
    geom_text(aes(label = value, y = value), size = 10, position = position_stack(vjust = 0.5))+
    theme(text = element_text(size=40),axis.text.x = element_text(angle = 45, hjust = 0.5, vjust=0.7)) 
  
  ggsave(file=paste0("~/R/contribution/",d_mahal[i,]$USUBJID,"_contr.png"), plot=image+labs(title = paste0("Pareto Plot showing contribution of each variable to distance\n",d_mahal[i,]$USUBJID)), width=20, height=16)

  ### next plot
  

  
  dist_tab <- d_mahal[, c(1, 2,3, 7:(ncol(d_mahal) - 3))]
  dist_tab <- d_mahal %>% select(c('USUBJID',as.vector(pcont2$variable[order(pcont2$variable)])))
  z <- d_mahal[i,]

  
  

  preprocessParams <- preProcess(dist_tab[,2:6], method=c("range"), rangeBounds = 0:1)
  normalized_dist_tab  <- predict(preprocessParams, dist_tab)
  
  dist_tab1 <-
    melt(normalized_dist_tab, id.vars = c( 'USUBJID'))
  dist_tab1 <- dist_tab1 %>%
    mutate(flag = ifelse(
      USUBJID == z$USUBJID,
      paste0('',z$USUBJID),
      paste0('Other Subjects')
    ))%>%
    mutate(sizedot = ifelse(
      USUBJID == z$USUBJID,
      100,
      1
    ))
  
  #whole table
  dist_tab2 <- dist_tab1 %>% filter(variable %in% pcont2$variable)
  #table for subject of interest
  dist_tab3 <-  melt(dist_tab %>%filter(USUBJID %in% z$USUBJID), id.vars = c( 'USUBJID'))
  #table for all other subjects
  # dist_tab4 <- dist_tab2 %>% filter(!(USUBJID %in% z$USUBJID))
  perc_cont_to_join <- pcont2 %>% select(USUBJID,variable,percont)
  dist_tab2 <- left_join(dist_tab2, dist_tab3, by=c("USUBJID", "variable"))
  dist_tab2 <- left_join(dist_tab2, perc_cont_to_join, by=c("USUBJID", "variable"))
  
  
  
  dist_tab_fact <- dist_tab2 %>% select(variable, percont) %>% distinct() %>% 
    filter(!is.na(percont)) %>% arrange(desc(percont))
  
  dist_tab2$variable <- factor(dist_tab2$variable, levels = dist_tab_fact$variable)
  
  p <- dist_tab2 %>% 
    ggplot(aes(
      x = variable,
      stroke = 3.0,
      y = value.x,
      fill = flag,
      color = flag,
      size = sizedot
    )) +
    geom_quasirandom() + geom_label_repel (
      size = 15,
      aes (label = ifelse (sizedot == 100, as.character(value.y), '')),
      nudge_x = 0.5,
      direction = "x",
      arrow = arrow (
        length = unit (0.005, "npc"),
        type = "open",
        ends = "last",
        angle = 45
      ),
      colour = "black",
      fill = "white"
    ) +
    xlab("Variable") + ylab("Normalised [0,1] value") + theme(text = element_text(size = 40),
  axis.text.x = element_text(
    angle = 45,
    hjust = 0.5,
    vjust = 0.7
  ))

  ggsave(file=paste0("~/R/contribution/",d_mahal[i,]$USUBJID,"_distribution.png"), plot=p+ labs(title = paste0("Distribution of ",d_mahal[i,]$USUBJID,"\nvariables(red) among others")), width=20, height=16)
  
  
  
}



library(tidyverse)
library(dplyr)
library(ggpubr)

library(OutlierDetection)


data <- read.csv("C:/Users/Yana/Documents/Hololens_MP/Assets/dimensionality_reduction/data/dist_m.csv")

data.num <- data %>% 
  select_if(is.numeric)
rownames(data.num) <- data.num[,1]

USUBJID <- data$USUBJID


# Function to generate control limits of Mahalanobis Distance from Beta Distribution
Mahal_CL <- function(p, m, alpha) {
  sqrt(((m - 1) ^ 2) / m * qbeta(1 - alpha, p / 2, (m - p - 1) / 2))
}


#===========PREPROCESSING========================

library(caret)

# scale and center each column (leave out columns X and distances)
preprocessParams <- preProcess(data.num[,2:122], method=c("center", "scale"))
# transform the dataset using the parameters
transformed <- predict(preprocessParams, data.num[,2:122])

# range everything to be from 0 to 1
#preprocessParams <- preProcess(transformed[,], method=c("range"))

# transform the dataset using the parameters
#transformed <- predict(preprocessParams, transformed[,])


#==============PCA===========================
data.pca <- read.csv("C:/Users/Yana/Documents/Hololens_MP/Assets/dimensionality_reduction/data/PCA_short_data.csv")
res.pca <- data.pca[,c('USUBJID','x1','y1','z1','MDistance')]

res <- res.pca[,c(2,3,4)]

library(OutlierDetection)

k_n <- 0.05 * nrow(data)

severe_inliers <- nn(res, k = k_n, cutoff = 0.05)
moderate_inliers <- nn(res, k = k_n, cutoff = 0.2)
severe_outliers <- nn(res, k = k_n, cutoff = 0.95)
moderate_outliers <- nn(res, k = k_n, cutoff = 0.8)

severe_inliers <- severe_inliers$`Location of Outlier`
moderate_inliers <- moderate_inliers$`Location of Outlier`
severe_outliers <- severe_outliers$`Location of Outlier`
moderate_outliers <- moderate_outliers$`Location of Outlier`


res.pca$Status = "Within Range"

for (i in 1:nrow(data)) {
  if (! i %in% moderate_inliers) {
    res.pca$Status[i] <- "Moderate Inlier"
  }
  if (! i %in% severe_inliers) {
    res.pca$Status[i] <- "Severe Inlier"
  }
  if (i %in% moderate_outliers) {
    res.pca$Status[i] <- "Moderate Outlier"
  }
  if (i %in% severe_outliers) {
    res.pca$Status[i] <- "Severe Outlier"
  }
}

#==========MULTIDIMENSIONAL SCALING==============

mds <- transformed %>%
  dist(method = 'manhattan') %>%
  cmdscale(k = 3) %>%
  as_tibble()


colnames(mds) <- c("Dim.1", "Dim.2", "Dim.3")

mahal.mds <- mahalanobis(mds, colMeans(mds), cov(mds))

res.mds <- data.frame(USUBJID) %>%
  mutate(x = mds$Dim.1) %>% 
  mutate(y = mds$Dim.2) %>% 
  mutate(z = mds$Dim.3) %>%
  mutate(MDistance = round(sqrt(mahal.mds), 3))

res <- res.mds[,c(2,3,4)]

library(OutlierDetection)

k_n <- 0.05 * nrow(data)

severe_inliers <- nn(res, k = k_n, cutoff = 0.05)
moderate_inliers <- nn(res, k = k_n, cutoff = 0.2)
severe_outliers <- nn(res, k = k_n, cutoff = 0.95)
moderate_outliers <- nn(res, k = k_n, cutoff = 0.8)

severe_inliers <- severe_inliers$`Location of Outlier`
moderate_inliers <- moderate_inliers$`Location of Outlier`
severe_outliers <- severe_outliers$`Location of Outlier`
moderate_outliers <- moderate_outliers$`Location of Outlier`


res.mds$Status = "Within Range"

for (i in 1:nrow(data)) {
  if (! i %in% moderate_inliers) {
    res.mds$Status[i] <- "Moderate Inlier"
  }
  if (! i %in% severe_inliers) {
    res.mds$Status[i] <- "Severe Inlier"
  }
  if (i %in% moderate_outliers) {
    res.mds$Status[i] <- "Moderate Outlier"
  }
  if (i %in% severe_outliers) {
    res.mds$Status[i] <- "Severe Outlier"
  }
}


# ggscatter(res, x = "x", y = "y",
#            # label = rownames(data),
#            # size = 1,
#            col = res$color)


#================Locally Linear Embedding===========


library(lle)
# neighbours <- find_nn_k(transformed, k=15)
# weights <- find_weights(neighbours, transformed, m=2, reg=2)

# calculation optimal number of neighbours k using Kayo algorithm
# k_opt <- calc_k(transformed, ncol(transformed), kmin=1, kmax=60, plotres=TRUE,  parallel=TRUE, cpus=2)

neighbours <- 0.08 * nrow(data)
lle.k5 <- lle(transformed, m=3, k = neighbours, reg=2, ss=FALSE, id=TRUE, v=0.9 )


lle <- data.frame(lle.k5$Y[,1], lle.k5$Y[,2], lle.k5$Y[,3])
colnames(lle) <- c("Dim.1", "Dim.2", "Dim.3")

mahal.lle <- mahalanobis(lle, colMeans(lle), cov(lle))

res.lle <- data.frame(USUBJID) %>%
  mutate(x = lle$Dim.1) %>% 
  mutate(y = lle$Dim.2) %>% 
  mutate(z = lle$Dim.3)  %>%
  mutate(MDistance = round(sqrt(mahal.lle), 3))

res <- res.lle[,c(2,3,4)]

k_n <- 0.05 * nrow(data)

severe_inliers <- nn(res, k = k_n, cutoff = 0.05)
moderate_inliers <- nn(res, k = k_n, cutoff = 0.2)
severe_outliers <- nn(res, k = k_n, cutoff = 0.95)
moderate_outliers <- nn(res, k = k_n, cutoff = 0.8)

severe_inliers <- severe_inliers$`Location of Outlier`
moderate_inliers <- moderate_inliers$`Location of Outlier`
severe_outliers <- severe_outliers$`Location of Outlier`
moderate_outliers <- moderate_outliers$`Location of Outlier`


res.lle$Status = "Within Range"

for (i in 1:nrow(data)) {
  if (! i %in% moderate_inliers) {
    res.lle$Status[i] <- "Moderate Inlier"
  }
  if (! i %in% severe_inliers) {
    res.lle$Status[i] <- "Severe Inlier"
  }
  if (i %in% moderate_outliers) {
    res.lle$Status[i] <- "Moderate Outlier"
  }
  if (i %in% severe_outliers) {
    res.lle$Status[i] <- "Severe Outlier"
  }
}

# res$color = "black"
# res$color[res$status == "Severe Outlier"] = "red"
# res$color[res$status == "Moderate Outlier"] = "pink"
# 
# ggscatter(res, x = "x", y = "y",
#           # label = rownames(data),
#           # size = 1,
#           col = res$color)


#===============MAKE FINAL DATA FRAME======================

res.pca <- res.pca %>%
  rename(MDistance1 = MDistance, Status1 = Status)

res.mds <- res.mds %>%
  rename(x2 = x, y2 = y, z2 = z, MDistance2 = MDistance, Status2 = Status)

res.lle <- res.lle %>%
  rename(x3 = x, y3 = y, z3 = z, MDistance3 = MDistance, Status3 = Status)

res.final <- merge(res.pca, res.mds, by = "USUBJID")
res.final <- merge(res.final, res.lle, by = "USUBJID")

write.csv(res.final, file="C:/Users/Yana/Documents/Hololens_MP/Assets/dimensionality_reduction//data/PCA_MDS_LLE.csv",
          row.names = TRUE,  quote = FALSE)


#==========2D======================

#============2D PCA=======================

res.pca <- data.pca[,c('USUBJID','x1','y1','MDistance')]

res <- res.pca[,c(2,3)]

library(OutlierDetection)

k_n <- 0.05 * nrow(data)

severe_inliers <- nn(res, k = k_n, cutoff = 0.05)
moderate_inliers <- nn(res, k = k_n, cutoff = 0.2)
severe_outliers <- nn(res, k = k_n, cutoff = 0.95)
moderate_outliers <- nn(res, k = k_n, cutoff = 0.8)

severe_inliers <- severe_inliers$`Location of Outlier`
moderate_inliers <- moderate_inliers$`Location of Outlier`
severe_outliers <- severe_outliers$`Location of Outlier`
moderate_outliers <- moderate_outliers$`Location of Outlier`


res.pca$Status = "Within Range"

for (i in 1:nrow(data)) {
  if (! i %in% moderate_inliers) {
    res.pca$Status[i] <- "Moderate Inlier"
  }
  if (! i %in% severe_inliers) {
    res.pca$Status[i] <- "Severe Inlier"
  }
  if (i %in% moderate_outliers) {
    res.pca$Status[i] <- "Moderate Outlier"
  }
  if (i %in% severe_outliers) {
    res.pca$Status[i] <- "Severe Outlier"
  }
}

#=====2D  MDS===========

mds_2d <- transformed %>%
  dist(method = 'manhattan') %>%
  cmdscale(k = 2) %>%
  as_tibble()


colnames(mds_2d) <- c("Dim.1", "Dim.2")

mahal.mds_2d <- mahalanobis(mds_2d, colMeans(mds_2d), cov(mds_2d))

res.mds_2d <- data.frame(USUBJID) %>%
  mutate(x = mds_2d$Dim.1) %>% 
  mutate(y = mds_2d$Dim.2) %>%
  mutate(MDistance = round(sqrt(mahal.mds_2d), 3))

res1 <- res.mds_2d[,c(2,3)]


k_n <- 0.05 * nrow(data)

severe_inliers <- nn(res1, k = k_n, cutoff = 0.05)
moderate_inliers <- nn(res1, k = k_n, cutoff = 0.2)
severe_outliers <- nn(res1, k = k_n, cutoff = 0.95)
moderate_outliers <- nn(res1, k = k_n, cutoff = 0.8)

severe_inliers <- severe_inliers$`Location of Outlier`
moderate_inliers <- moderate_inliers$`Location of Outlier`
severe_outliers <- severe_outliers$`Location of Outlier`
moderate_outliers <- moderate_outliers$`Location of Outlier`


res.mds_2d$Status = "Within Range"

for (i in 1:nrow(data)) {
  if (! i %in% moderate_inliers) {
    res.mds_2d$Status[i] <- "Moderate Inlier"
  }
  if (! i %in% severe_inliers) {
    res.mds_2d$Status[i] <- "Severe Inlier"
  }
  if (i %in% moderate_outliers) {
    res.mds_2d$Status[i] <- "Moderate Outlier"
  }
  if (i %in% severe_outliers) {
    res.mds_2d$Status[i] <- "Severe Outlier"
  }
}


#==============2D LLE============================

neighbours <- 0.08 * nrow(data)
lle.k5 <- lle(transformed, m=2, k = neighbours, reg=2, ss=FALSE, id=TRUE, v=0.9 )


lle <- data.frame(lle.k5$Y[,1], lle.k5$Y[,2])
colnames(lle) <- c("Dim.1", "Dim.2")

mahal.lle <- mahalanobis(lle, colMeans(lle), cov(lle))

res.lle <- data.frame(USUBJID) %>%
  mutate(x = lle$Dim.1) %>% 
  mutate(y = lle$Dim.2) %>%
  mutate(MDistance = round(sqrt(mahal.lle), 3))

res <- res.lle[,c(2,3)]

k_n <- 0.05 * nrow(data)

severe_inliers <- nn(res, k = k_n, cutoff = 0.05)
moderate_inliers <- nn(res, k = k_n, cutoff = 0.2)
severe_outliers <- nn(res, k = k_n, cutoff = 0.95)
moderate_outliers <- nn(res, k = k_n, cutoff = 0.8)

severe_inliers <- severe_inliers$`Location of Outlier`
moderate_inliers <- moderate_inliers$`Location of Outlier`
severe_outliers <- severe_outliers$`Location of Outlier`
moderate_outliers <- moderate_outliers$`Location of Outlier`


res.lle$Status = "Within Range"

for (i in 1:nrow(data)) {
  if (! i %in% moderate_inliers) {
    res.lle$Status[i] <- "Moderate Inlier"
  }
  if (! i %in% severe_inliers) {
    res.lle$Status[i] <- "Severe Inlier"
  }
  if (i %in% moderate_outliers) {
    res.lle$Status[i] <- "Moderate Outlier"
  }
  if (i %in% severe_outliers) {
    res.lle$Status[i] <- "Severe Outlier"
  }
}

#===============MAKE FINAL DATA FRAME======================

res.pca <- res.pca %>%
  rename(MDistance1 = MDistance, Status1 = Status)

res.mds_2d <- res.mds_2d %>%
  rename(x2 = x, y2 = y, MDistance2 = MDistance, Status2 = Status)

res.lle <- res.lle %>%
  rename(x3 = x, y3 = y, MDistance3 = MDistance, Status3 = Status)

res.final <- merge(res.pca, res.mds_2d, by = "USUBJID")
res.final <- merge(res.final, res.lle, by = "USUBJID")

write.csv(res.final, file="C:/Users/Yana/Documents/Hololens_MP/Assets/dimensionality_reduction/data/2D_PCA_MDS_LLE.csv",
          row.names = TRUE,  quote = FALSE)

