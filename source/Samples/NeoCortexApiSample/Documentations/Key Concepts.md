# Key Concepts: ML 24/25-01 Investigate Image Reconstruction by using Classifiers

## Table of Contents

- [Binarization](#binarization)
- [Sparse Distributed Representations (SDR)](#sparse-distributed-representations-sdr)
- [Hierarchical Temporal Memory (HTM)](#hierarchical-temporal-memory-htm)
- [Spatial Pooler (SP)](#spatial-pooler-sp)
- [HTM Classifiers](#htm-classifiers)
- [KNN Classifiers](#knn-classifiers)
- [Difference between HTM and KNN Classifiers](#difference-between-htm-and-knn-classifiers)

### This section describes the key concepts behind this experiment :

### Binarization
The Image Binarizer is a utility that converts grayscale or colored images into binary format (black and white) by applying a thresholding technique. This process simplifies image data by representing pixels as either 0 (black) or 1 (white), making it suitable for Sparse Distributed Representations (SDRs) used in Hierarchical Temporal Memory (HTM) models. The binarized images are then processed by the Spatial Pooler (SP), which learns patterns and helps in classification and prediction tasks, making it essential for image recognition and anomaly detection in HTM-based applications. https://www.nuget.org/packages/ImageBinarizer

### Sparse Distributed Representations (SDR)

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

### Spatial Pooler (SP)

The Spatial Pooler (SP) is a core element of Hierarchical Temporal Memory (HTM) systems, converting raw input data into Sparse Distributed Representations (SDRs). It ensures key properties like sparsity, which improves efficiency and reduces noise sensitivity, and similarity preservation, where similar inputs produce overlapping SDRs for effective pattern recognition. Through competition among columns of cells, guided by adaptive synaptic connections, the SP learns the statistical structure of the input space, making it robust to noise and capable of generalization. This forms the foundation for advanced processes like temporal learning and classification. https://www.frontiersin.org/journals/computational-neuroscience/articles/10.3389/fncom.2017.00111/full

### HTM Classifiers
Hierarchical Temporal Memory (HTM) classifiers are essential for sequence learning and pattern recognition within the HTM framework. They link Sparse Distributed Representations (SDRs) from the Spatial Pooler and Temporal Memory to meaningful labels. During training, the classifier maps SDRs to output labels and, once trained, can predict the most likely label for new or partial inputs. Unlike traditional classifiers, HTM classifiers are biologically inspired, allowing them to handle noise, detect anomalies, and generalize from sparse inputs. Their incremental learning capability makes them ideal for real-time tasks like image recognition and time-series forecasting. https://github.com/Avradip24/Code_Wizards/blob/master/source/Documentation/htm-classifier.md 

### KNN Classifiers
The KNN classifier is a simple, non-parametric algorithm that classifies new data points based on their proximity to labeled training samples. When provided with an SDR or a feature vector, KNN calculates the distance to the "k" nearest neighbors and assigns the most frequent label. In this project, KNN can serve as a baseline classifier to compare HTM's generalization capabilities with KNN’s similarity-based approach. However, KNN lacks the adaptive learning ability of HTM, making it less effective for evolving data streams.During classification, it computes the distance between an unknown input and stored SDRs, ranking results by similarity. https://scikit-learn.org/stable/modules/neighbors.html

### Difference between HTM and KNN Classifiers
The primary distinction between Hierarchical Temporal Memory (HTM) and K-Nearest Neighbors (KNN) lies in their learning approach, adaptability, and handling of high-dimensional Sparse Distributed Representations (SDRs).

- *Learning Paradigm:* HTM is a biologically inspired model that learns spatial and temporal patterns dynamically through unsupervised learning, while KNN is a non-parametric, supervised algorithm that relies on labeled data for classification without adapting to new data over time.
- *Adaptability:* HTM continuously updates its learning as new data arrives, making it ideal for evolving data streams. In contrast, KNN remains static unless manually updated.
- *Handling High-Dimensional Data:* HTM effectively processes sparse, high-dimensional SDRs, while KNN struggles with the "curse of dimensionality," leading to reduced efficiency and accuracy.
- *Pattern Reconstruction:* HTM's spatial pooler can reconstruct input patterns using learned synaptic connections, while KNN lacks this capability, focusing solely on classification.
- *Similarity Metrics:* HTM leverages metrics like the Jaccard Index for evaluating overlap between original and reconstructed SDRs, while KNN only provides insights into neighbor-based influence. 
