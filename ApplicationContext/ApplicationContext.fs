namespace DMLib_WPF.Contexts

open DMLib

type ApplicationContext() =
    inherit WPFBindable()

    member val WindowsSound = WindowsSoundPlayer() with get, set
