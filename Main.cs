using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using WindowsInput;

namespace UNDERTALEBot
{
	public class Program
	{
		public static DiscordSocketClient _client;
		public static Program Instance;

		private InputSimulator sim;

		public static Task Main(string[] args) => new Program().MainAsync();

		public Program()
		{
			Program.Instance = this;
			sim = new InputSimulator();
		}

		private Task Log(LogMessage msg)
		{
			Console.WriteLine(msg.ToString());
			return Task.CompletedTask;
		}

		private async Task<Task> MessageSent(SocketMessage message)
		{
			// Failsaves for non-user messages
			if (!(message is SocketUserMessage userMessage)) return Task.CompletedTask;
			if (userMessage.Source != MessageSource.User) return Task.CompletedTask;

			// Garbage code that handles user inputs.
			WindowsInput.Native.VirtualKeyCode keyToRelease = WindowsInput.Native.VirtualKeyCode.VK_0;
			
			switch (userMessage.Content.ToLower())
            {
				case "z":
					sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_Z);
					keyToRelease = WindowsInput.Native.VirtualKeyCode.VK_Z;
					break;
				case "x":
					sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_X);
					keyToRelease = WindowsInput.Native.VirtualKeyCode.VK_X;
					break;
				case "c":
					sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.VK_C);
					keyToRelease = WindowsInput.Native.VirtualKeyCode.VK_C;
					break;
				case "up":
					sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.UP);
					keyToRelease = WindowsInput.Native.VirtualKeyCode.UP;
					break;
				case "down":
					sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.DOWN);
					keyToRelease = WindowsInput.Native.VirtualKeyCode.DOWN;
					break;
				case "left":
					sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.LEFT);
					keyToRelease = WindowsInput.Native.VirtualKeyCode.LEFT;
					break;
				case "right":
					sim.Keyboard.KeyDown(WindowsInput.Native.VirtualKeyCode.RIGHT);
					keyToRelease = WindowsInput.Native.VirtualKeyCode.RIGHT;
					break;
			}

			await Task.Delay(200);
			sim.Keyboard.KeyUp(keyToRelease);

			return Task.CompletedTask;
		}

		public async Task MainAsync()
		{
			_client = new DiscordSocketClient();
			_client.Log += Log;
			_client.MessageReceived += MessageSent;

			if (!File.Exists(Path.Combine(Environment.CurrentDirectory, "Config", "token.txt")))
			{
				Console.WriteLine("Token not found. Make sure to input your bots token in config/token.txt!");
				return;
			}

			var token = File.ReadAllText(Path.Combine(Environment.CurrentDirectory, "config", "token.txt"));

			await _client.SetGameAsync("UNDERTALE", null, ActivityType.Playing); // Fake undertale game activity
			await _client.LoginAsync(TokenType.Bot, token);
			await _client.StartAsync();

			await Task.Delay(-1);
		}
	}
}