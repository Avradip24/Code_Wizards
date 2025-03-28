# Project Title: ML 24/25-01 Investigate Image Reconstruction by using Classifiers

# Developed in [**C#**](https://learn.microsoft.com/en-us/dotnet/csharp/), with ❤️

### An experiment to showcase the prediction and reconstruction of images using Sparse Distributed Representations (SDRs) through HTM and KNN classifiers in C#

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

Refer to the [Key Concepts](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/Documentations/Key%20Concepts.md) document to read about the core concepts behind this experiment.

### Project Architecture:

![Overview of the Project (2)](https://github.com/user-attachments/assets/7bed04c0-6e72-47c7-819b-4f7028373367)

This image represents a flowchart describing an image processing, classification and reconstruction pipeline. It consists of the following components:
- **Input Image**: An initial dataset of image is provided as input.
- **Image Binarizer**: Converts the image into a binarized format (0 & 1).
- **HTM Spatial Pooler**: Processes the binarized image to generate a Sparse Distributed Representation (SDR).
- **Classification & Reconstruction**:
- *HTM Classifier*: Uses the SDR to learn and predict the image and reconstruct the predicted image.
- *KNN Classifier*: Another classification method that also reconstructs the predicted image.
- **Comparison 1**: The reconstructed images from both classifiers are compared with the original binarized image to evaluate their accuracy.
- **Comparison 2**: The reconstructed images from both classifiers are compared with each other to assess differences in classification performance.

### Getting Started:

Refer to the [Getting Started](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/Documentations/Getting%20Started.md) document to know more about how to run the experiment.


### Project Overview

The project highlight the comparative performance of HTM (Hierarchical Temporal Memory) and KNN (k-Nearest Neighbors) classifiers in image recognition, prediction, and reconstruction. The system first captures all the provided images of various extensions from the specific folder and split them randomly into Training (80%) and Testing (20%) Images. Then binarizes both the sets of images and trains both classifiers on Spatial Pooler (SP) generated Sparse Distributed Representations (SDRs) for training images. During prediction phase, binarized test images are processed through the HTM and KNN classifiers, predicting the closest matching images based on similarity scores. The predicted SDRs are then reconstructed into binarized images, and their accuracy is evaluated using Jaccard Similarity and Hamming Distance Similarity against the original binarized images. The reconstruction times are also measured for performance assessment.

The results indicate that both classifiers perform uniquely based on the test images, with HTM sometimes outperforming KNN and vice versa. The project generates graphical visualizations, such as Similarity Graphs for Jaccard Similarity and Scott plots for Hamming Distance Similarity, to analyze the effectiveness of each classifier in reconstructing images. Also the comparison between HTM and KNN reconstructed images is done to understand which classifier preserves the original structure better. Finally, for all the predictions and reconstruction which classifier overall performed better is evaluated. Ultimately, the findings provide insights into the strengths and weaknesses of each approach, aiding in selecting the best-suited classifier for image recognition and reconstruction tasks in future applications.  

### Methodology

There are various Methods used in the Experiment keeping in mind the concept of code reusabilty. Refer to the [Methodology](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/Documentations/Methodology.md) for code overview.

### Results

Refer to the [Project Findings](https://github.com/Avradip24/Code_Wizards/blob/master/source/Samples/NeoCortexApiSample/Documentations/ProjectFindings.md) for results and outcomes. 
