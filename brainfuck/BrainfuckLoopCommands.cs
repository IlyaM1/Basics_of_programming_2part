using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
        public static void RegisterTo(IVirtualMachine vm)
		{
			FindLeftAndRightBrackets(vm);

            vm.RegisterCommand('[', b => {
				if (vm.Memory[vm.MemoryPointer] == 0)
				{
                    var tuple = FindLeftAndRightBrackets(vm);
                    var left = tuple.Item1;
                    var right = tuple.Item2;
                    vm.InstructionPointer = right[left.IndexOf(vm.InstructionPointer)];
                }
					
			});
            vm.RegisterCommand(']', b => {
                if (vm.Memory[vm.MemoryPointer] != 0)
				{
                    var tuple = FindLeftAndRightBrackets(vm);
                    var left = tuple.Item1;
                    var right = tuple.Item2;
                    var IP = vm.InstructionPointer;
					var index = right.IndexOf(IP);
					var leftBracket = left[index];
                    vm.InstructionPointer = leftBracket;
                } 
                    
            });
        }

		private static Tuple<List<int>, List<int>> FindLeftAndRightBrackets(IVirtualMachine vm)
		{
            var _leftBrackets = new List<int>();
            var _rightBrackets = new List<int>();

			//var commands = vm.Instructions.Replace('\r', ' ').Replace('\n', ' ').Replace(" ", "");
			var commands = vm.Instructions;

            var left = 0;
			var right = commands.Length - 1;

			while (left < right)
			{
				if (commands[left] == '[')
				{
					_leftBrackets.Add(left);
					while (left < right)
					{
						if (commands[right] == ']')
						{
							_rightBrackets.Add(right);
							break;
						}
						right--;
					}
				}
				left++;
			}

			return new Tuple<List<int>, List<int>>(_leftBrackets, _rightBrackets);
        }
	}
}