using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => { write((char)vm.Memory[vm.MemoryPointer]); });
			vm.RegisterCommand('+', b => { unchecked { vm.Memory[vm.MemoryPointer] += 1; } });
			vm.RegisterCommand('-', b => { unchecked { vm.Memory[vm.MemoryPointer] -= 1; }});
            vm.RegisterCommand(',', b => { vm.Memory[vm.MemoryPointer] = (byte)read(); });

            vm.RegisterCommand('>', b => { 
                vm.MemoryPointer += 1;
                if (vm.MemoryPointer >= vm.Memory.Length)
                    vm.MemoryPointer = 0;
            });
            vm.RegisterCommand('<', b => { 
                vm.MemoryPointer -= 1;
                if (vm.MemoryPointer < 0)
                    vm.MemoryPointer = vm.Memory.Length - 1;
            });

            var allowableSymbols = GenerateAllowableSymbols();
            foreach (var symbol in allowableSymbols)
				vm.RegisterCommand(symbol, b => { vm.Memory[vm.MemoryPointer] = (byte)symbol; });
        }

		public static string GenerateAllowableSymbols()
		{
			var symbolsString = new StringBuilder();
            for (var i = (int)(byte)('A'); i <= (byte)('Z'); i++)
            {
                var currentSymbol = (char)i;
				symbolsString.Append(currentSymbol);
            }
            for (var i = (int)(byte)('a'); i <= (byte)('z'); i++)
            {
                var currentSymbol = (char)i;
                symbolsString.Append(currentSymbol);
            }
            for (var i = (int)(byte)('0'); i <= (byte)('9'); i++)
            {
                var currentSymbol = (char)i;
                symbolsString.Append(currentSymbol);
            }
            return symbolsString.ToString();
        }
	}
}