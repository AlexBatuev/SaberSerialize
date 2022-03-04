using System;
using System.IO;
using System.Text;

namespace SaberSerialize
{
    public class Program
    {
        private static readonly Random Rand = new();

        private static ListNode CreateNode(ListNode prevNode, string data)
        {
            var node = new ListNode
            {
                Prev = prevNode,
                Data = data
            };
            return node;
        }

        private static void AppendNode(ListRand list, ListNode newNode)
        {
            list.Tail.Next = newNode;
            newNode.Prev = list.Tail;
            newNode.Next = null;
            list.Tail = newNode;
            list.Count++;
        }

        private static void AssignRandomNodeFromList(ListRand list, ListNode node)
        {
            if (list.Count < 2)
                return;

            if (Rand.Next(0, 2) == 0)
                return;

            var rand = node;
            while (rand == node)
            {
                rand = GetRandomNodeFromList(list);
            }
  
            node.Rand = rand;
        }

        private static ListNode GetRandomNodeFromList(ListRand list)
        {
            var random = Rand.Next(0, list.Count);
            var currentNode = list.Head;
            var step = 1;
            while (step++ < random)
            {
                currentNode = currentNode.Next;
            }

            return currentNode;
        }

        private static string CreateString(int length = 8)
        {
            var builder = new StringBuilder();

            for (var i = 0; i < length; i++)
            {
                var flt = Rand.NextDouble();
                var shift = Convert.ToInt32(Math.Floor(25 * flt));
                var letter = Convert.ToChar(shift + 65);
                builder.Append(letter);
            }

            return builder.ToString();
        }

        private static bool CompareLists(ListRand list1, ListRand list2)
        {
            if (list1.Count != list2.Count)
                return false;

            var node1 = list1.Head;
            var node2 = list2.Head;

            for (var i = 0; i < list1.Count; i++)
            {
                if (node1.Data != node2.Data || (node1.Rand != node2.Rand && node1.Rand.Data != node2.Rand.Data))
                    return false;

                node1 = node1.Next;
                node2 = node2.Next;
            }

            return true;
        }

        public static void Main()
        {
            var originalList = new ListRand();
            var head = CreateNode(null, CreateString());
            originalList.Head = originalList.Tail = head;
            originalList.Count = 1;

            var random = Rand.Next(9, 99);
            for (var i = 0; i < random; i++)
            {
                var node = CreateNode(originalList.Tail, CreateString());
                AppendNode(originalList, node);
            }

            var current = originalList.Head;
            while (current != null)
            {
                AssignRandomNodeFromList(originalList, current );
                current = current.Next;
            }

            using (var fs = new FileStream("ListRand.srl", FileMode.Create))
            {
                originalList.Serialize(fs);
            }

            var serializedList = new ListRand();
            using (var fs = new FileStream("ListRand.srl", FileMode.Open))
            {
                serializedList.Deserialize(fs);
            }

            Console.WriteLine(CompareLists(originalList, serializedList)
                ? "The data match"
                : "The data does not match");
            Console.ReadKey();
        }
    }
}
