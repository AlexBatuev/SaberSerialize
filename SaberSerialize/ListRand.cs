using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace SaberSerialize
{
    // Реализуйте функции сериализации и десериализации двусвязного списка, заданного следующим образом:
    // Примечание: сериализация подразумевает сохранение и восстановление полной структуры списка,
    // включая взаимное соотношение его элементов между собой — в том числе ссылок на Rand элементы.

    class ListNode
    {
        public ListNode Prev;
        public ListNode Next;
        public ListNode Rand; // произвольный элемент внутри списка
        public string Data;
    }

    class ListRand
    {
        public ListNode Head;
        public ListNode Tail;
        public int Count;

        public void Serialize(FileStream s)
        {
            var nodes = new List<ListNode>();
            var current = Head;

            while (current != null)
            {
                nodes.Add(current);
                current = current.Next;
            }

            using var sw = new StreamWriter(s);
            foreach (var node in nodes)
            {
                sw.WriteLine($"{node.Data}:{nodes.IndexOf(node.Rand)}");
            }
        }

        public void Deserialize(FileStream s)
        {
            Count = 0;
            var nodes = new List<ListNode>();
            var current = new ListNode();
            Head = current;

            try
            {
                using (var sr = new StreamReader(s))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line != "")
                        {
                            Count++;
                            current.Data = line;
                            var next = new ListNode();
                            current.Next = next;
                            next.Prev = current;
                            nodes.Add(current);
                            current = next;
                        }
                    }
                }

                Tail = current.Prev;
                Tail.Next = null;

                foreach (var node in nodes)
                {
                    var separator = node.Data.LastIndexOf(':');
                    var index = Convert.ToInt32(node.Data[(separator + 1)..]);
                    node.Rand = index != -1 ? nodes[index] : null;
                    node.Data = node.Data[..(separator)];
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Deserialization error!");
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                Console.ReadKey();
                Environment.Exit(0);
            }
        }
    }
}
