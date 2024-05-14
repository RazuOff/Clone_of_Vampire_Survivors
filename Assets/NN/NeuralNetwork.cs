using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

[System.Serializable]
public class NeuralNetworkListData
{
  public List<NeuralNetworkData> networkDataList;

  public NeuralNetworkListData(List<NeuralNetwork> neuralNetworks)
  {
    networkDataList = new List<NeuralNetworkData>();
    foreach (var neuralNetwork in neuralNetworks)
    {
      NeuralNetworkData data = new NeuralNetworkData
      {
        layers = neuralNetwork.layers,
        flatNeurons = FlattenArray(neuralNetwork.neurons),
        flatBiases = FlattenArray(neuralNetwork.biases),
        flatWeights = FlattenArray(neuralNetwork.weights),
        activations = neuralNetwork.activations,
        fitness = neuralNetwork.fitness
      };
      networkDataList.Add(data);
    }
  }

  private float[] FlattenArray(float[][] array)
  {
    List<float> flatList = new List<float>();
    foreach (var subArray in array)
    {
      flatList.AddRange(subArray);
    }
    return flatList.ToArray();
  }

  private float[] FlattenArray(float[][][] array)
  {
    List<float> flatList = new List<float>();
    foreach (var subArray2D in array)
    {
      foreach (var subArray in subArray2D)
      {
        flatList.AddRange(subArray);
      }
    }
    return flatList.ToArray();
  }
  





}
[System.Serializable]
public class NeuralNetworkData
{
  public int[] layers;
  public float[] flatNeurons;
  public float[] flatBiases;
  public float[] flatWeights;
  public int[] activations;
  public float fitness;
}

[System.Serializable]
public class NeuralNetwork : IComparable<NeuralNetwork>
{
  public int[] layers;//layers
  public float[][] neurons;//neurons
  public float[][] biases;//biasses
  public float[][][] weights;//weights
  public int[] activations;//layers

  public float fitness = 0;//fitness

  public NeuralNetwork()
  {

  }

  public NeuralNetwork(int[] layers)
  {
    this.layers = new int[layers.Length];
    for (int i = 0; i < layers.Length; i++)
    {
      this.layers[i] = layers[i];
    }
    InitNeurons();
    InitBiases();
    InitWeights();
  }


  


  private void InitNeurons()//create empty storage array for the neurons in the network.
  {
    List<float[]> neuronsList = new List<float[]>();
    for (int i = 0; i < layers.Length; i++)
    {
      neuronsList.Add(new float[layers[i]]);
    }
    neurons = neuronsList.ToArray();
  }

  private void InitBiases()//initializes and populates array for the biases being held within the network.
  {
    List<float[]> biasList = new List<float[]>();
    for (int i = 0; i < layers.Length; i++)
    {
      float[] bias = new float[layers[i]];
      for (int j = 0; j < layers[i]; j++)
      {
        bias[j] = UnityEngine.Random.Range(-0.5f, 0.5f);
      }
      biasList.Add(bias);
    }
    biases = biasList.ToArray();
  }

  private void InitWeights()//initializes random array for the weights being held in the network.
  {
    List<float[][]> weightsList = new List<float[][]>();
    for (int i = 1; i < layers.Length; i++)
    {
      List<float[]> layerWeightsList = new List<float[]>();
      int neuronsInPreviousLayer = layers[i - 1];
      for (int j = 0; j < neurons[i].Length; j++)
      {
        float[] neuronWeights = new float[neuronsInPreviousLayer];
        for (int k = 0; k < neuronsInPreviousLayer; k++)
        {
          //float sd = 1f / ((neurons[i].Length + neuronsInPreviousLayer) / 2f);
          neuronWeights[k] = UnityEngine.Random.Range(-0.5f, 0.5f);
        }
        layerWeightsList.Add(neuronWeights);
      }
      weightsList.Add(layerWeightsList.ToArray());
    }
    weights = weightsList.ToArray();
  }

  public float[] FeedForward(float[] inputs)//feed forward, inputs >==> outputs.
  {
    for (int i = 0; i < inputs.Length; i++)
    {
      neurons[0][i] = inputs[i];
    }
    for (int i = 1; i < layers.Length; i++)
    {
      int layer = i - 1;
      for (int j = 0; j < neurons[i].Length; j++)
      {
        float value = 0f;
        for (int k = 0; k < neurons[i - 1].Length; k++)
        {
          value += weights[i - 1][j][k] * neurons[i - 1][k];
        }
        neurons[i][j] = activate(value + biases[i][j]);
      }
    }
    return neurons[neurons.Length - 1];
  }

  public float activate(float value)
  {
    return (float)Math.Tanh(value);
  }

  public void Mutate(float chance, float val)//used as a simple mutation function for any genetic implementations.
  {
    for (int i = 0; i < biases.Length; i++)
    {
      for (int j = 0; j < biases[i].Length; j++)
      {
        biases[i][j] = (UnityEngine.Random.value <= chance) ? biases[i][j] += UnityEngine.Random.Range(-val, val) : biases[i][j];
      }
    }

    for (int i = 0; i < weights.Length; i++)
    {
      for (int j = 0; j < weights[i].Length; j++)
      {
        for (int k = 0; k < weights[i][j].Length; k++)
        {
          weights[i][j][k] = (UnityEngine.Random.value <= chance) ? weights[i][j][k] += UnityEngine.Random.Range(-val, val) : weights[i][j][k];
        }
      }
    }
  }

  public int CompareTo(NeuralNetwork other) //Comparing For NeuralNetworks performance.
  {
    if (other == null) return 1;

    if (fitness > other.fitness)
      return 1;
    else if (fitness < other.fitness)
      return -1;
    else
      return 0;
  }

  public NeuralNetwork copy(NeuralNetwork nn) //For creatinga deep copy, to ensure arrays are serialzed.
  {
    for (int i = 0; i < biases.Length; i++)
    {
      for (int j = 0; j < biases[i].Length; j++)
      {
        nn.biases[i][j] = biases[i][j];
      }
    }
    for (int i = 0; i < weights.Length; i++)
    {
      for (int j = 0; j < weights[i].Length; j++)
      {
        for (int k = 0; k < weights[i][j].Length; k++)
        {
          nn.weights[i][j][k] = weights[i][j][k];
        }
      }
    }
    return nn;
  }

  
}