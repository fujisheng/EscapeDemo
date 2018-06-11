using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tools.Mathf{
    public class RandomNumber
    {
        public static List<int> GetNumbers(int number,int min,int max){
            List<int> numbers = new List<int>();
            int index = 0;
            while (true)
            {
                int num = Random.Range(min, max + 1);
                if (!numbers.Contains(num))
                {
                    numbers.Add(num);
                    index++;
                }
                if (index >= number || number > (max - min + 1))
                    break;
            }
            return numbers;
        }
    }
}

