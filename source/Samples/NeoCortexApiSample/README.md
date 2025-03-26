# Project Title: ML 24/25-01 Investigate Image Reconstruction by using Classifiers

## Table of Contents
- [Problem Statement](#problem-statement)
- [Introduction](#introduction)
- [Key Concepts](#key-concepts)
- [Project Architecture](#project-architecture)
- [Getting Started](#getting-started)
- [Project Overview](#project-overview)
- [Methodology](#methodology)
- [Results](#results)
### Problem Statement

Image reconstruction is a fundamental challenge in artificial intelligence and machine learning, particularly when dealing with sparse representations such as Sparse Distributed Representations (SDRs). The study in this project seeks to answer key questions: How well do the HTM and KNN classifier preserve structural details during reconstruction? What are their respective strengths and weaknesses in terms of accuracy and efficiency? Addressing these questions will provide insights into their applicability in image reconstruction tasks for real-world use cases such as pattern recognition, anomaly detection, and predictive modeling.

### Introduction:

This project aims to explore the role of classifiers in Hierarchical Temporal Memory (HTM) systems, focusing on their ability to associate input patterns with meaningful predictions and reconstruct original inputs from Sparse Distributed Representations (SDRs). By investigating and comparing two existing classifiers, HTM and KNN, the project seeks to evaluate their functionality, performance, and differences. The HTM Classifier leverages the principles of temporal memory within HTM, while the KNN classifier employs a distance-based approach to classify SDRs based on nearest neighbours. Inspired by the SpatialLearning experiment, a new experiment has been implemented to regenerate input images from SDRs produced by the Spatial Pooler (SP), leveraging the IClassifier interface for learning and prediction. The experiment will use the ImageBinarizer to binarize input images, predict and reconstruct inputs via classifiers, and compare them with the originals using similarity measures. Results has been illustrated with diagrams, analysed quantitatively, and discussed, providing insights into the prediction and reconstruction capabilities of classifiers.

### Key Concepts:

Refer to [this documents]() to read about the key concepts behind this experiment.

### Project Architecture:

![Overview of the Project (2)](https://github.com/user-attachments/assets/7bed04c0-6e72-47c7-819b-4f7028373367)

### Getting Started:

Refer to [this document]() to know more about how to run the experiment.


### Project Overview

The project highlight the comparative performance of HTM (Hierarchical Temporal Memory) and KNN (k-Nearest Neighbors) classifiers in image recognition, prediction, and reconstruction. The system first captures all the provided images of various extensions from the specific folder and split them randomly into Training (80%) and Testing (20%) Images. Then binarizes both the sets of images and trains both classifiers on Spatial Pooler (SP) generated Sparse Distributed Representations (SDRs) for training images. During prediction phase, binarized test images are processed through the HTM and KNN classifiers, predicting the closest matching images based on similarity scores. The predicted SDRs are then reconstructed into binarized images, and their accuracy is evaluated using Jaccard Similarity and Hamming Distance Similarity against the original binarized images. The reconstruction times are also measured for performance assessment.

The results indicate that both classifiers perform uniquely based on the test images, with HTM sometimes outperforming KNN and vice versa. The project generates graphical visualizations, such as Similarity Graphs for Jaccard Similarity and Scott plots for Hamming Distance Similarity, to analyze the effectiveness of each classifier in reconstructing images. Also the comparison between HTM and KNN reconstructed images is done to understand which classifier preserves the original structure better. Finally, for all the predictions and reconstruction which classifier overall performed better is evaluated. Ultimately, the findings provide insights into the strengths and weaknesses of each approach, aiding in selecting the best-suited classifier for image recognition and reconstruction tasks in future applications.  

### Methodology

There are various Methods used in the Experiment keeping in mind the concept of code reusabilty. Refere to [this document]() for code overview.

### Results

Refer to [this document]() for results and outcomes. 
