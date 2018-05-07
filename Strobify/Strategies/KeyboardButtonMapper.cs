namespace Strobify.Strategies
{
    using Strategies.Interfaces;
    using WindowsInput.Native;

    public class KeyboardButtonMapper : IKeyboardButtonMapper
    {
        public VirtualKeyCode SetVirtualKeyCode(string buttonKey)
        {
            switch (buttonKey.ToUpper())
            {
                case "A":
                    return VirtualKeyCode.VK_A;
                case "B":
                    return VirtualKeyCode.VK_B;
                case "C":
                    return VirtualKeyCode.VK_C;
                case "D":
                    return VirtualKeyCode.VK_D;
                case "E":
                    return VirtualKeyCode.VK_E;
                case "F":
                    return VirtualKeyCode.VK_F;
                case "G":
                    return VirtualKeyCode.VK_G;
                case "H":
                    return VirtualKeyCode.VK_H;
                case "I":
                    return VirtualKeyCode.VK_I;
                case "J":
                    return VirtualKeyCode.VK_J;
                case "K":
                    return VirtualKeyCode.VK_K;
                case "L":
                    return VirtualKeyCode.VK_L;
                case "M":
                    return VirtualKeyCode.VK_M;
                case "N":
                    return VirtualKeyCode.VK_N;
                case "O":
                    return VirtualKeyCode.VK_O;
                case "P":
                    return VirtualKeyCode.VK_P;
                case "R":
                    return VirtualKeyCode.VK_R;
                case "S":
                    return VirtualKeyCode.VK_S;
                case "T":
                    return VirtualKeyCode.VK_T;
                case "U":
                    return VirtualKeyCode.VK_U;
                case "W":
                    return VirtualKeyCode.VK_W;
                case "X":
                    return VirtualKeyCode.VK_X;
                case "Y":
                    return VirtualKeyCode.VK_Y;
                case "Z":
                    return VirtualKeyCode.VK_Z;

                default:
                    return VirtualKeyCode.VK_L;
            }
        }
    }
}
