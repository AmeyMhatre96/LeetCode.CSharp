using Solutions;
using System;
using System.Data.Common;
using System.Data.Common;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text;

namespace myspace;

public static class Extensions
{
    //public static int[] ForEachWithIndex(this int[] ints, Action<int, int> action)
    //{
    //    int index = 0;
    //    foreach (var i in ints)
    //    {
    //        action()
    //        index++;
    //    }
    //}
}

public abstract class Problem
{
    public abstract void Execute();

    public static int[] NewArray(params int[] ints)
    {
        return ints;
    }

    public static void AssertEqual(int[] one, int[] two)
    {
        if (string.Join('.', one) != string.Join('.', two))
        {
            throw new Exception();
        }
    }

    public static void AssertEqual(int one, int two)
    {
        if (one != two)
        {
            throw new Exception();
        }
    }
}

public class TwoSumProblem
{
    public class TestData
    {
        public int Target { get; set; }

        public int[] InputNumbers { get; set; }

        public int[] ExpectedIndices { get; set; }
    }
    public static void Execute()
    {
        var datas = new List<TestData>
            {
                new TestData
                {
                    Target = 11,
                    InputNumbers = ArrayOf(1,1,1,1,1,4,1,1,1,1,1,7,1,1,1,1,1),
                    ExpectedIndices = ArrayOf(0,1)
                },
                new TestData
                {
                    Target = 9,
                    InputNumbers = ArrayOf(2,7,11,15),
                    ExpectedIndices = ArrayOf(0,1)
                },
                new TestData
                {
                    Target = 6,
                    InputNumbers = ArrayOf(3,2,4),
                    ExpectedIndices = ArrayOf(1,2)
                },
                new TestData
                {
                    Target = 6,
                    InputNumbers = ArrayOf(3,3),
                    ExpectedIndices = ArrayOf(0,1)
                }
            };

        foreach (var data in datas)
        {
            var res = TwoSum(data.InputNumbers, data.Target);
            if (res.Length == data.ExpectedIndices.Length)
            {
                for (int i = 0; i < res.Length; i++)
                {
                    if (res[i] != data.ExpectedIndices[i])
                    {
                        throw new Exception("Wrong");
                    }
                }
            }
            else
            {
                throw new Exception("Wrong");
            }
        }

    }

    static int[] ArrayOf(params int[] entries)
    {
        return entries;
    }

    static public int[] TwoSum(int[] nums, int target)
    {
        Dictionary<int, int> ComplementHash = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            int currentVal = nums[i];
            int complement = target - currentVal;
            if (ComplementHash.ContainsKey(complement))
            {
                return new int[] { ComplementHash[complement], i };
            }
            else
            {
                ComplementHash.TryAdd(currentVal, i);
            }
        }

        return null;
    }

    static public int[] TwoSum1(int[] nums, int target)
    {
        int num1 = 0, num2 = 0;
        bool found = false;
        for (int i = 0; i < nums.Length - 1; i++)
        {
            for (int j = i + 1; j < nums.Length; j++)
            {
                if (nums[i] + nums[j] == target)
                {
                    num1 = i;
                    num2 = j;
                    found = true;
                    break;
                }
            }
            if (found) break;
        }
        return ArrayOf(num1, num2);
    }
}

public class MaxSubArrayProblem : Problem
{
    public class TestData
    {
        public int Output { get; set; }

        public int[] InputNumbers { get; set; }

        public TestData(int output, params int[] inputs)
        {
            Output = output;
            InputNumbers = inputs;
        }
    }

    // Find the maximum possible sum in arr[]
    // such that arr[m] is part of it
    static int maxCrossingSum(int[] arr, int l, int m,
                              int h)
    {
        // Include elements on left of mid.
        int sum = 0;
        int left_sum = int.MinValue;
        for (int i = m; i >= l; i--)
        {
            sum = sum + arr[i];
            if (sum > left_sum)
                left_sum = sum;
        }

        // Include elements on right of mid
        sum = 0;
        int right_sum = int.MinValue;
        ;
        for (int i = m; i <= h; i++)
        {
            sum = sum + arr[i];
            if (sum > right_sum)
                right_sum = sum;
        }

        // Return sum of elements on left
        // and right of mid
        // returning only left_sum + right_sum will fail for
        // [-2, 1]
        return Math.Max(left_sum + right_sum - arr[m],
                        Math.Max(left_sum, right_sum));
    }

    // Returns sum of maximum sum subarray
    // in aa[l..h]
    static int maxSubArraySum(int[] arr, int l, int h)
    {
        //Invalid Range: low is greater than high
        if (l > h)
            return int.MinValue;
        // Base Case: Only one element
        if (l == h)
            return arr[l];

        // Find middle point
        int m = (l + h) / 2;

        /* Return maximum of following three
        possible cases:
        a) Maximum subarray sum in left half
        b) Maximum subarray sum in right half
        c) Maximum subarray sum such that the
        subarray crosses the midpoint */
        return Math.Max(
            Math.Max(maxSubArraySum(arr, l, m - 1),
                     maxSubArraySum(arr, m + 1, h)),
            maxCrossingSum(arr, l, m, h));
    }


    int MaxSubArray(int[] nums)
    {
        if (nums.Length == 0)
        {
            return 0;
        }
        int currentSum = 0;
        int maxSum = currentSum = nums[0];
        for (int i = 1; i < nums.Length; i++)
        {
            var thisNumber = nums[i];
            if (currentSum < 0 && thisNumber > currentSum)
            {
                currentSum = thisNumber;
            }
            else
            {
                currentSum += thisNumber;
            }
            if (currentSum > maxSum)
            {
                maxSum = currentSum;
            }
        }
        return maxSum;
    }

    public override void Execute()
    {
        List<TestData> testDatas = new List<TestData>
            {
                new TestData(output: 6, -2,1,-3,4,-1,2,1,-5,4),
                new TestData(output: 1, 1),
                new TestData(output: 23, 5,4,-1,7,8),
            };
        foreach (var item in testDatas)
        {
            //if(MaxSubArray(item.InputNumbers) != item.Output)
            if (maxSubArraySum(item.InputNumbers, 0, item.InputNumbers.Length - 1) != item.Output)
            {
                throw new Exception();
            }
        }
    }
}

public class MoveZerosProblem : Problem
{
    public void MoveZeroes(int[] nums)
    {
        int zerosToAddAtEnd = 0;
        for (int i = 0; i < nums.Length; i++)
        {
            if (nums[i] == 0)
            {
                zerosToAddAtEnd++;
            }
            else if (zerosToAddAtEnd != 0)
            {
                nums[i - zerosToAddAtEnd] = nums[i];
            }
        }
        if (zerosToAddAtEnd != 0)
        {
            for (int i = nums.Length - zerosToAddAtEnd; i < nums.Length; i++)
            {
                nums[i] = 0;
            }
        }
    }

    public override void Execute()
    {
        List<TestData> testDatas = new List<TestData>
            {
                new TestData(input: NewArray(0,1), output: NewArray(1,0)),
                new TestData(input: NewArray(0,1,0,3,12), output: NewArray(1,3,12,0,0)),
                new TestData(input: NewArray(0), output: NewArray(0)),
            };
        foreach (var item in testDatas)
        {
            MoveZeroes(item.Input);
            AssertEqual(item.Input, item.Output);
        }
    }

    protected class TestData
    {
        public int[] Input { get; set; }

        public int[] Output { get; set; }

        public TestData(int[] input, int[] output)
        {
            Input = input;
            Output = output;
        }
    }

}

public class ContainsDuplicateProblem : Problem
{
    public class TestData
    {
        public int[] Input { get; set; }

        public bool Output { get; set; }

        public TestData(int[] input, bool output)
        {
            Input = input;
            Output = output;
        }
    }
    public bool ContainsDuplicate(int[] nums)
    {
        // Use hashset instead.
        Dictionary<int, int> discoveredValues = new Dictionary<int, int>();
        for (int i = 0; i < nums.Length; i++)
        {
            if (discoveredValues.ContainsKey(nums[i]))
            {
                return true;
            }
            else
            {
                discoveredValues.Add(nums[i], i);
            }
        }

        return false;
    }

    public override void Execute()
    {
        List<TestData> testDatas = new List<TestData>
            {
                new TestData(input: NewArray(1,2,3,1), output: true),
                new TestData(input: NewArray(1,2,3,4), output: false),
                new TestData(input: NewArray(1,1,1,3,3,4,3,2,4,2), output: true),
            };
        foreach (var item in testDatas)
        {
            if (ContainsDuplicate(item.Input) != item.Output)
            {
                throw new Exception();
            }
        }
    }
}

/// <summary>
/// https://leetcode.com/problems/rotate-array/
/// </summary>
public class RotateArrayProblem : Problem
{
    public class TestData
    {
        public int[] Input { get; set; }

        public int Steps { get; set; }

        public int[] Output { get; set; }

        public TestData(int[] input, int steps, int[] output)
        {
            Input = input;
            Steps = steps;
            Output = output;
        }
    }

    public void RotateArray(int[] nums, int k)
    {
        if (nums == null || nums.Length == 1)
        {
            return;
        }
        if (k >= nums.Length)
        {
            k %= nums.Length;
        }
        if (k == 0)
        {
            return;
        }

        int[] rotatedNums = new int[nums.Length];
        for (int i = 0; i < nums.Length; i++)
        {
            int distanceFromEnd = nums.Length - i;
            int targetIndex = distanceFromEnd > k ?
                i + k : // To right
                k - distanceFromEnd; // From start

            rotatedNums[targetIndex] = nums[i];

        }
        for (int i = 0; i < nums.Length; i++)
        {
            nums[i] = rotatedNums[i];
        }
    }
    static readonly Dictionary<int, int> PreviousValues = new();

    public void RotateArrayInplace(int[] nums, int k)
    {
        if (nums == null || nums.Length == 1)
        {
            return;
        }
        if (k >= nums.Length)
        {
            k %= nums.Length;
        }
        if (k == 0)
        {
            return;
        }
        PreviousValues.Clear();
        for (int i = 0; i < nums.Length; i++)
        {
            int distanceFromEnd = nums.Length - i;
            int targetIndex = distanceFromEnd > k ?
                i + k : // To right
                k - distanceFromEnd; // From start
            PreviousValues.Add(targetIndex, nums[targetIndex]);
            if (PreviousValues.TryGetValue(i, out int valueToSet))
            {
                nums[targetIndex] = valueToSet;
            }
            else
            {
                nums[targetIndex] = nums[i];
            }

        }
    }

    public override void Execute()
    {
        List<TestData> testDatas = new List<TestData>
            {
                new TestData(input: NewArray(-1), steps: 2, output:  NewArray(-1)),
                new TestData(input: NewArray(-1, 1), steps: 2, output:  NewArray(-1, 1)),
                new TestData(input: NewArray(-1, 1), steps: 3, output:  NewArray(1, -1)),
                new TestData(input: NewArray(1, 2), steps: 5, output:  NewArray(2, 1)),
                new TestData(input: NewArray(1,2,3,4,5,6,7), steps: 3, output:  NewArray(5,6,7,1,2,3,4)),
                new TestData(input: NewArray(-1,-100,3,99), steps: 2, output:  NewArray(3,99,-1,-100))
            };
        foreach (var item in testDatas)
        {
            RotateArrayInplace(item.Input, item.Steps);
            AssertEqual(item.Input, item.Output);
        }
    }
}

/// <summary>
/// https://leetcode.com/problems/add-two-numbers
/// </summary>
public class AddTwoNumbersProblem : Problem
{
    public class ListNode
    {
        public int val;
        public ListNode? next;
        public ListNode(int val = 0, ListNode? next = null)
        {
            this.val = val;
            this.next = next;
        }

        public static ListNode? FromInts(params int[] ints)
        {
            ListNode? node = null;

            if (ints.Length > 0)
            {
                ListNode current = new();
                for (int i = 0; i < ints.Length; i++)
                {
                    node ??= current;
                    current.val = ints[i];
                    if (i < ints.Length - 1)
                    {
                        current.next = new ListNode();
                        current = current.next;
                    }
                }
            }

            return node;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            ListNode? current = this;
            while (current != null)
            {
                stringBuilder.Append(current.val + ";");
                current = current.next;
            }
            return stringBuilder.ToString();
        }
    }


    public class TestData
    {
        public ListNode L1 { get; set; }
        public ListNode L2 { get; set; }

        public ListNode Output { get; set; }

        public TestData(ListNode L1, ListNode L2, ListNode Output)
        {
            this.L1 = L1;
            this.L2 = L2;
            this.Output = Output;
        }
    }
    public ListNode? AddTwoNumbers(ListNode l1, ListNode l2)
    {
        if (l1 == null && l2 == null)
        {
            return null;
        }
        ListNode output = new(0);
        ListNode currentOutput = output;

        int carry = 0;
        ListNode? currentL1 = l1;
        ListNode? currentL2 = l2;
        while (currentL1 != null || currentL2 != null)
        {
            var sum = (currentL1 != null ? currentL1.val : 0) + (currentL2 != null ? currentL2.val : 0) + carry;
            if (sum >= 10)
            {
                carry = sum / 10;
                sum %= 10;
            }
            else
            {
                carry = 0;
            }
            currentOutput.val = sum;
            if (currentL1?.next != null || currentL2?.next != null)
            {
                currentOutput.next = new ListNode();
                currentOutput = currentOutput.next;
            }
            currentL1 = currentL1?.next;
            currentL2 = currentL2?.next;
        }
        if (carry != 0)
        {
            currentOutput.next = new ListNode(carry);
        }

        return output;
    }



    public override void Execute()
    {
        List<TestData> testDatas = new List<TestData>
            {
                new TestData(L1: ListNode.FromInts(2,4,3), L2: ListNode.FromInts(5,6,4),Output: ListNode.FromInts(7,0,8)),
                new TestData(L1: ListNode.FromInts(0), L2: ListNode.FromInts(0),Output: ListNode.FromInts(0)),
                new TestData(L1: ListNode.FromInts(9,9,9,9,9,9,9), L2: ListNode.FromInts(9,9,9,9),Output: ListNode.FromInts(8,9,9,9,0,0,0,1)),
            };
        foreach (var item in testDatas)
        {
            var actual = AddTwoNumbers(item.L1, item.L2)?.ToString();
            if (actual != item.Output.ToString())
            {
            }
        }
    }
}

/// <summary>
/// https://leetcode.com/problems/longest-substring-without-repeating-characters/
/// </summary>
public class LongestSubscringWithoutRepeatingCharsProblem : Problem
{
    public class TestData
    {
        public string Input { get; set; }

        public int Output { get; set; }

        public TestData(string input, int output)
        {
            Input = input;
            Output = output;
        }
    }

    public int LengthOfLongestSubstringUsingString(string s)
    {
        string longestString = string.Empty;
        string currentString = string.Empty;
        Dictionary<char, int> charIndexes = new();
        for (int i = 0; i < s.Length; i++)
        {
            char currentChar = s[i];
            if (charIndexes.ContainsKey(currentChar))
            {
                //currentString = currentString.Remove(charIndexes[currentChar], 1);
                currentString = currentString.Remove(0, currentString.IndexOf(currentChar) + 1);
                currentString += currentChar;
                //charIndexes[currentChar] = currentString.Length - 1;
            }
            else
            {
                currentString += currentChar;
                charIndexes.Add(currentChar, currentString.Length - 1);
            }
            if (currentString.Length > longestString.Length)
            {
                longestString = currentString;
            }
        }

        return longestString.Length;
    }

    public int LengthOfLongestSubstring(string s)
    {
        string longestString = string.Empty;
        int startIndex = 0;
        int endIndex = 0;
        Dictionary<char, int> charIndexes = new();
        for (int i = 0; i < s.Length; i++)
        {
            char currentChar = s[i];
            if (charIndexes.TryGetValue(currentChar, out int index))
            {
                if (index >= startIndex)
                {
                    startIndex = charIndexes[currentChar] + 1;
                }
                charIndexes[currentChar] = i;
            }
            else
            {
                charIndexes.Add(currentChar, endIndex);
            }
            endIndex++;
            string sub = s.Substring(startIndex, endIndex - startIndex);
            if (sub.Length > longestString.Length)
            {
                longestString = sub;
            }
        }

        return longestString.Length;
    }

    public override void Execute()
    {
        List<TestData> testDatas = new List<TestData>
        {
            new TestData(input: "aabaab!bb", output: 3),
            new TestData(input: "abba", output: 2),
            new TestData(input: "abcabcbb", output: 3),
            new TestData(input: "bbbbb", output: 1),
            new TestData(input: "pwwkew", output: 3),
        };
        foreach (var item in testDatas)
        {
            var result = LengthOfLongestSubstring(item.Input);
            AssertEqual(result, item.Output);
        }
    }
}

class Program
{
    static void Main(string[] _)
    {
        //DS.Perform();
        Problem problem = new LongestSubscringWithoutRepeatingCharsProblem();
        problem.Execute();
    }
}
