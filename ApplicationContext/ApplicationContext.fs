namespace DMLib_WPF.Contexts

open DMLib
open System.Windows

type ApplicationContext() =
    inherit WPFBindable()

    member val OwnerWindow: Window = null with get, set
    member val WindowsSound = WindowsSoundPlayer() with get, set
