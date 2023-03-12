using System.Collections.Generic;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static Dictionary<int, int> LeftToRightBrackets;
        public static Dictionary<int, int> RightToLeftBrackets;

        public static void RegisterTo(IVirtualMachine vm)
        {
            FindBrackets(vm);

            vm.RegisterCommand('[', b => {
                if (vm.Memory[vm.MemoryPointer] == 0)
                    vm.InstructionPointer = LeftToRightBrackets[vm.InstructionPointer];
            }
            );
            vm.RegisterCommand(']', b => {
                if (vm.Memory[vm.MemoryPointer] != 0)
                    vm.InstructionPointer = RightToLeftBrackets[vm.InstructionPointer];
            });
        }

        private static void FindBrackets(IVirtualMachine vm)
        {
            LeftToRightBrackets= new Dictionary<int, int>();
            RightToLeftBrackets = new Dictionary<int, int>();

            var leftBrackets = new Stack<int>();
            var i = 0;
            while (i < vm.Instructions.Length)
            {
                if (vm.Instructions[i] == '[')
                    leftBrackets.Push(i);
                else if (vm.Instructions[i] == ']')
                {
                    var leftBracketPopped = leftBrackets.Pop();
                    LeftToRightBrackets.Add(leftBracketPopped, i);
                    RightToLeftBrackets.Add(i, leftBracketPopped);
                }
                i++;
            }
        }
    }
}