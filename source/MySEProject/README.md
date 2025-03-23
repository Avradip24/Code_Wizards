# Project Title: ML 24/25-01 Investigate Image Reconstruction by using Classifiers

## Table of Contents

- [Problem Statement](#problem-statement)
- [Introduction](#introduction)
- [The Overview of the Project](#the-overview-of-the-project)
- [Image Encoder](#image-encoder)
- [Sparse Distributed Representations (SDR)](#sparse-distributed-representations-sdr)
- [Hierarchical Temporal Memory (HTM)](#hierarchical-temporal-memory-htm)
- [Spatial Pooler (SP)](#spatial-pooler-sp)
  - [Spatial Pooler Functions](#spatial-pooler-functions)
- [HTM Classifiers](#htm-classifiers)
- [KNN Classifiers](#knn-classifiers)
- [Difference between HTM and KNN Classifiers](#difference-between-htm-and-knn-classifiers)
- [Methodology](#methodology)
- [Project Findings](#project-findings)


### Problem Statement:
This project aims to explore the role of classifiers in Hierarchical Temporal Memory (HTM) systems,
focusing on their ability to associate input patterns with meaningful predictions and reconstruct
original inputs from Sparse Distributed Representations (SDRs). By investigating and comparing two
existing classifiers, Htm and KNN, the project seeks to evaluate their functionality,
performance, and differences. Inspired by the SpatialLearning experiment, a new experiment will be
implemented to regenerate input images from SDRs produced by the Spatial Pooler (SP), leveraging
the IClassifier interface for learning and prediction. The experiment will use the ImageEncoder to
process images, predict and reconstruct inputs via classifiers, and compare them with the originals using
similarity measures. Results will be illustrated with diagrams, analysed quantitatively, and discussed,
providing insights into the reconstruction capabilities of classifiers.

### Introduction:
This project explores the integration and application of classifiers within the Hierarchical Temporal Memory (HTM) framework to regenerate input data from Sparse Distributed Representations (SDRs). The core goal is to understand the role of classifiers in reverse encoding, where the learned SDR representations are used to reconstruct the original input. Through this process, we aim to analyse the behaviour and performance of two existing classifiers— Htm and KNN to implement a new experiment that leverages their capabilities. The two classifiers under study in this project—HTMClassifier and KNN—serve as foundational implementations. The HTMClassifier leverages the principles of temporal memory within HTM, while the KNN classifier employs a distance-based approach to classify SDRs based on nearest neighbours. Through this investigation, the project bridges the gap between abstract HTM theories and practical applications, contributing to advancements in intelligent systems and neural computation.

### The Overview of the Project:
![Copy of Copy of Untitled presentation](https://github.com/user-attachments/assets/224f852d-f240-4f4b-8954-3b9e31eec1ed)

### Image Encoder:
The ImageEncoder plays a crucial role in preparing image data for processing within Hierarchical
Temporal Memory (HTM) systems by converting raw image inputs into binary representations
compatible with HTM's Sparse Distributed Representations (SDRs). Based on the ImageBinarizer
NuGet package, the ImageEncoder encodes pixel intensity or feature information into a format that
preserves essential patterns while reducing redundancy. This encoding ensures that similar images
produce similar SDRs, a key characteristic that enables effective learning and pattern recognition in
HTM systems. By preprocessing images into this sparse binary format, the ImageEncoder bridges the
gap between raw image data and the HTM's Spatial Pooler, making it a foundational component for
image-based experiments, such as learning spatial patterns or regenerating inputs from SDRs. https://www.nuget.org/packages/ImageBinarizer

### Sparse Distributed Representations (SDR):

Sparse Distributed Representations (SDRs) are analogous to how the human brain encodes
information. Just as neurons in the brain fire in sparse patterns, with only a small fraction of neurons
active at any time, SDRs use binary vectors where a small percentage of bits are active (1s) while the
rest are inactive (0s). In the brain, these sparse activations ensure energy efficiency and robustness,
as the overlap in neural firing patterns helps identify similar stimuli. Similarly, SDRs are sparse to
reduce computational complexity and distributed to make the system resilient to noise or
corruption. Technically, SDRs preserve key properties of input data, such as similarity and
distinctiveness, through overlapping active bits for similar inputs and distinct patterns for dissimilar
inputs. This allows HTM systems to efficiently encode, recognize, and generalize patterns, just as the
brain does when processing sensory input.https://www.frontiersin.org/journals/computational-neuroscience/articles/10.3389/fncom.2017.00111/full

### Hierarchical Temporal Memory (HTM)

Hierarchical Temporal Memory is a theoretical framework and machine learning approach inspired by the structure and function of the neocortex in the human brain. Temporal Memory (TM) is a key component of HTM. It is the mechanism responsible for learning and predicting sequences of patterns based on temporal context.The workflow of Hierarchical Temporal Memory (HTM) involves processing input data into sparse distributed representations (SDRs) using an encoder, which captures the essential features of the input.https://www.numenta.com/blog/2019/10/24/machine-learning-guide-to-htm/ 

### Spatial Pooler (SP):

The Spatial Pooler (SP) is a core element of Hierarchical Temporal Memory (HTM) systems, converting raw input data into Sparse Distributed Representations (SDRs). It ensures key properties like sparsity, which improves efficiency and reduces noise sensitivity, and similarity preservation, where similar inputs produce overlapping SDRs for effective pattern recognition. Through competition among columns of cells, guided by adaptive synaptic connections, the SP learns the statistical structure of the input space, making it robust to noise and capable of generalization. This forms the foundation for advanced processes like temporal learning and classification.

### Spatial Pooler Functions

- Converts binarized image inputs into a high-dimensional numerical format.
- Activates a small set of columns to efficiently use memory and recognize patterns.
- Strengthens connections to frequently active input bits for stable pattern recognition.
- Ensures similar inputs generate similar SDRs while handling noise and minor variations.
- Maintains network stability and prevents excessive changes.
- Uses learned SDRs for classification and prediction tasks. https://www.frontiersin.org/journals/computational-neuroscience/articles/10.3389/fncom.2017.00111/full

### HTM Classifiers:
Hierarchical Temporal Memory (HTM) classifiers are essential for sequence learning and pattern recognition within the HTM framework. They link Sparse Distributed Representations (SDRs) from the Spatial Pooler and Temporal Memory to meaningful labels. During training, the classifier maps SDRs to output labels and, once trained, can predict the most likely label for new or partial inputs. Unlike traditional classifiers, HTM classifiers are biologically inspired, allowing them to handle noise, detect anomalies, and generalize from sparse inputs. Their incremental learning capability makes them ideal for real-time tasks like image recognition and time-series forecasting. https://numenta.com/neuroscience-research/

### KNN Classifiers:
The KNN classifier is a simple, non-parametric algorithm that classifies new data points based on their proximity to labeled training samples. When provided with an SDR or a feature vector, KNN calculates the distance to the "k" nearest neighbors and assigns the most frequent label. In this project, KNN can serve as a baseline classifier to compare HTM's generalization capabilities with KNN’s similarity-based approach. However, KNN lacks the adaptive learning ability of HTM, making it less effective for evolving data streams.During classification, it computes the distance between an unknown input and stored SDRs, ranking results by similarity.https://scikit-learn.org/stable/ 

### Difference between HTM and KNN Classifiers:
The primary distinction between Hierarchical Temporal Memory (HTM) and K-Nearest Neighbors (KNN) lies in their learning approach, adaptability, and handling of high-dimensional Sparse Distributed Representations (SDRs).

- **Learning Paradigm:** HTM is a biologically inspired model that learns spatial and temporal patterns dynamically through unsupervised learning, while KNN is a non-parametric, supervised algorithm that relies on labeled data for classification without adapting to new data over time.
- **Adaptability:** HTM continuously updates its learning as new data arrives, making it ideal for evolving data streams. In contrast, KNN remains static unless manually updated.
- **Handling High-Dimensional Data:** HTM effectively processes sparse, high-dimensional SDRs, while KNN struggles with the "curse of dimensionality," leading to reduced efficiency and accuracy.
- **Pattern Reconstruction:** HTM's spatial pooler can reconstruct input patterns using learned synaptic connections, while KNN lacks this capability, focusing solely on classification.
- **Similarity Metrics:** HTM leverages metrics like the Jaccard Index for evaluating overlap between original and reconstructed SDRs, while KNN only provides insights into neighbor-based influence. https://www.numenta.com/

### Methodology
 There are various Methods used in the Experiment keeping in mind the concept of code reusabilty:
- **1. Program.cs** - This file serves as the entry point for the Image Reconstruction experiment, which investigates the effectiveness of classifiers (HTM and KNN) in reconstructing images using Sparse Distributed Representations (SDRs) and the Spatial Pooler (SP) from the NeoCortex API. This file initializes and triggers the core experiment by invoking the ImageBinarizerSpatialPattern class. Upon execution, the program first displays a welcome message, introducing the experiment's purpose and acknowledging the contributors. This introductory section provides clarity on the objective of the experiment—predicting and reconstructing images using machine learning classifiers.

  Following the introduction, the Main method creates an instance of the ImageBinarizerSpatialPattern class and calls its Run() method, which begins the 
  experimental process. This experiment follows a structured pipeline that includes image binarization, Spatial Pooling algorithm, generation of SDRs, training of   classifiers (HTM & KNN), prediction of images, 
  and image reconstruction to assess the accuracy of the classifiers. By modularizing the functionality, Program.cs   ensures code reusability and maintainability, allowing the experiment to be extended or 
  modified with ease. The final results, including similarity comparisons    between the original and reconstructed images, are computed and stored for analysis.

  The significance of Program.cs lies in its role as the driver of the experiment, ensuring a seamless and structured execution. It abstracts away complex 
  processes by delegating core functionalities to ImageBinarizerSpatialPattern, making the codebase more readable and modular. By running this file, users can 
  initiate the complete workflow, from image preprocessing to final evaluation, in a streamlined manner. Future enhancements to the experiment can be integrated 
  without modifying Program.cs, reinforcing its role as a flexible and scalable entry point.

- **2. ImageBinarizerSpatialPattern.cs** - The ImageBinarizerSpatialPattern class is designed to handle the Spatial Pooling algorithm, training classifiers, prediction of images, and reconstruction of images using two different classifiers: Hierarchical Temporal Memory (HTM) and K-Nearest Neighbors (KNN). During the training phase, the class processes a dictionary of binarized images, where each image is represented as a Sparse Distributed Representation (SDR) consisting of active cells. The Spatial Pooler (SP) plays a key role here by performing the initial transformation of input patterns (the binarized images) into SDRs, where the SP selects a subset of columns in a sparse space that maximally represent the input image. These SDRs are then fed into both classifiers (HTM and KNN), enabling them to learn associations between the encoded representations and the original image labels. The dataset is split into 80% for training and 20% for testing, ensuring a proper evaluation of the classifiers' performance. The process is logged for tracking, and training times are measured for performance evaluation. The SP is crucial as it helps to create consistent, sparse, and distributed representations of the input data, enabling both classifiers to learn and predict effectively.

  In the prediction and reconstruction phase, the class takes a list of binarized testing images and processes them through a Spatial Pooler (SP) to extract 
  active columns, which serve as input for the classifiers. Both HTM and KNN make predictions by matching the SDRs of test images to the closest learned 
  representations. The highest similarity predictions from each classifier are selected, and the corresponding SDRs are reconstructed back into images using an 
  Image Reconstructor. The reconstructed images are then compared to the original binarized images, and similarity scores are calculated to assess reconstruction 
  accuracy.

  To provide insights into classifier performance, the class generates similarity graphs and plots for HTM and KNN reconstructions using ScottPlot and NeoCortex 
  utilities Similarity Graph. It also compares the reconstructed images visually and numerically to determine which classifier performed better for each test        image. The results are logged, detailing the 
  prediction accuracy of each classifier. Additionally, the class manages the organization of output files by           creating dedicated directories for reconstructed images and similarity plots, deleting any 
  previous results to maintain a clean experiment setup. Finally, to      ensure each experiment starts fresh, it resets the classifiers after each prediction cycle, clearing their internal states for the next 
  test.

- **3. BinarizeImage.cs** - This class is designed to convert an image into a structured binary text representation, making it suitable for further processing in machine learning models that rely on Sparse Distributed Representations (SDRs), such as Hierarchical Temporal Memory (HTM). The class provides a method, BinarizeImages, which takes in the desired image dimensions, the path of the input image, and the destination path for the binarized output. The method ensures that the output file is unique by appending a .txt extension to the specified destination and deleting any existing file with the same name to prevent conflicts.

  To perform the binarization, the method initializes an instance of the ImageBinarizer class with specific parameters that define how the image is processed. It 
  sets red, green, and blue threshold values to 200, meaning that any pixel with RGB values exceeding these thresholds is considered active and assigned a binary 
  value of 1, while all other pixels are marked as inactive with a 0. The image is also resized to the specified width and height to maintain consistency in input 
  dimensions, ensuring compatibility with downstream processing models.

  Once the binarization is complete, the method applies an inversion step where all 0s are flipped to 1s and vice versa. This transformation is performed to 
  ensure that the output aligns with the expected input format required for SDR-based models. The inverted binary data is then written back to the file, replacing 
  the original content.

  Finally, the method returns the path of the binarized text file, which now contains a structured binary representation of the original image. This output can be 
  used in various applications, including spatial pooling, pattern recognition, and anomaly detection, where SDRs play a crucial role in encoding information 
  efficiently.

- **4. ImageReconstruction.cs** - The ImageReconstructor.cs class reconstructs images from Sparse Distributed Representations (SDRs) using predicted active columns from classifiers. It extracts active columns, estimates permanence values with the Spatial Pooler (SP), normalizes them into a binary format, and saves the output as a text file. Inactive columns are assigned a permanence value of 0.0 to maintain consistency. The reconstructed images are stored in separate folders (ReconstructedHTM and ReconstructedKNN) for performance comparison between the HTM and KNN classifiers. Also the Similarity between the original and reconstructed image is calculated here by JaccardSimilarityofBinaryArrays. This process visualizes how accurately the classifiers predict original images based on learned SDR patterns.

- **5. htmClassifier.cs/knnClassifer.cs/IClassifier.cs** - These classes implements the classifiers, htmClassifier and knnClassifier, for image pattern recognition using Sparse Distributed Representations (SDRs) from the NeoCortex API. Both classifiers adhere to the IClassifier interface, which defines methods for learning input patterns, predicting future inputs, clearing internal state, and reconstructing inputs from predictions.

### Project Findings
This project focuses on implementing image prediction and reconstruction along with similarity analysis using HTM and KNN classifiers.

- The resultant Comparisons of similarities(Jaccard Index) of the Original Binarized image and the reconstructed image for both the classifier. The result is also plotted with a Scott Plot and Bar Graph for visualisation.
- The resultant Comparion of performance of both the Classifier to understand the which classifer performs better.  
