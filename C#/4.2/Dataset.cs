﻿/*
Copyright(c) 2015, Alysson Ribeiro da Silva All rights reserved.

Redistribution and use in source and binary forms, with or without
modification, are permitted provided that the following conditions are met :

1. Redistributions of source code must retain the above copyright notice, this
list of conditions and the following disclaimer.
2. Redistributions in binary form must reproduce the above copyright notice,
this list of conditions and the following disclaimer in the documentation
and / or other materials provided with the distribution.

THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
DISCLAIMED.IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR
ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
(INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
(INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

The views and conclusions contained in the software and documentation are those
of the authors and should not be interpreted as representing official policies,
either expressed or implied, of the FreeBSD Project.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace KNN
{
    public class Dataset
    {
        List<Instance> instances;

        public Dataset(String fileName, char delimiter)
        {
            instances = new List<Instance>();
            String line;
            Instance instance;
            try
            {

                using (StreamReader sr = new StreamReader(fileName))
                {
                    while ((line = sr.ReadLine()) != null)
                    {
                        //Console.WriteLine(line);
                        String[] data = line.Split(delimiter);
                        instance = new Instance(data);
                        instances.Add(instance);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }
           
        }


        /// <summary>
        /// Return all Instance
        /// </summary>
        /// <returns></returns>
        public List<Instance> getInstances()
        {
            return instances;
        }

        public List<Instance> getFirstK(int k)
        {
            List<Instance> firstk = new List<Instance>();
            for (int i = 0; i < k; i++)
            {
                firstk.Add(instances.ElementAt(i));
            }

            return firstk;
        }


        /// <summary>
        /// Sorts the dataset using distance value from a Instance target
        /// </summary>
        public void sort()
        {
            instances.Sort();
        }


        
        /// <summary>
        /// Gets the first Instance
        /// </summary>
        /// <returns></returns>
        public Instance getFirst()
        {
            return instances.First();
        }
    }
}
