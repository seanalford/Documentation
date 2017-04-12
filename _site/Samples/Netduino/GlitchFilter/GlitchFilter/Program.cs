﻿using System;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace GlitchFilter
{
	public class Program
	{
		// An output port allows you to write (send a signal) to a pin
		static OutputPort _led = new OutputPort(Pins.ONBOARD_LED, false);
		// An input port reads the signal from a pin (Should be Pins.ONBOARD_BTN, but there is a bug)
		static InputPort _button = new InputPort((Cpu.Pin)0x15, true, Port.ResistorMode.Disabled);

		public static void Main()
		{
			// turn the LED off initially
			_led.Write(false);

			// smooth noise out over 5 milliseconds
			Cpu.GlitchFilterTime = new TimeSpan(0,0,0,0,5);


			// run forever
			while (true)
			{
				// set the onboard LED output to be the input of the button
				_led.Write(_button.Read());
			}

		}
	}
}
