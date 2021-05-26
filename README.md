# NET Reactor Cflow Cleaner 6.7.X.X  


<p align="center">
  <img src="https://github.com/Hussaryn/NET-Reactor-Cflow-Cleaner-6.7.0.0/blob/main/Images/img.png?raw=true" />
</p>

---

## What is .NET Reactor?

.NET Reactor is a powerful code protection and software licensing system for software written for the .NET Framework, and supports all languages that generate .NET assemblies. 

## What is Control Flow?

Control Flow Obfuscation converts the code inside your methods into spaghetti code, which whilst retaining the function of the code makes it extremely difficult for human eyes and decompilers to follow the program logic. Decompilers are not able to decompile the spaghetti code back to your original source code.

---

### Before

```C#
public static void Main(string[] args)
{
    int num = 1;
    for (;;)
	{
        int num2 = num;
        do
        {
			switch (num2)
			{
				case 1:
					Console.WriteLine("Hello, type anything");
					num2 = 0;
					if (<Module>{ae7ca312-4aae-4a50-8c02-68cd0f6a49a5}.m_ac846466e909461989ac23502fbfe894 == 0)
					{
						goto Block_2;
					}
					continue;
				case 2:
                    return;
			}
			Console.ReadKey();
			num2 = 2;
		}
		while (<Module>{ae7ca312-4aae-4a50-8c02-68cd0f6a49a5}.m_d61b15f7b2a5404db656ec8afd85287e == 0);
		Block_2:;
	}
}
```
### After
```C#
public static void Main(string[] args)
{
    Console.WriteLine("Hello, type anything.");
    Console.ReadKey();
}
```
---

## References
> https://github.com/de4dot/de4dot/tree/master/de4dot.blocks <br>
> https://github.com/SychicBoy/.NetReactorCfCleaner

## Credits

- GitHub - [SychicBoy](https://github.com/SychicBoy)
- GitHub - [de4dot](https://github.com/de4dot)

[Back To The Top](#read-me-template)
