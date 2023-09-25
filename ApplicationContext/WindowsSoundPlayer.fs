namespace DMLib_WPF.Contexts

open System.Media

type SoundEffect =
    | NoSound
    | Success
    | Hint
    | Error

type WindowsSoundPlayer() =
    [<CompiledName("Play")>]
    member _.play sound =
        (match sound with
         | NoSound -> fun () -> ()
         | Success -> SystemSounds.Exclamation.Play
         | Error -> SystemSounds.Hand.Play
         | Hint -> SystemSounds.Asterisk.Play)
            ()

    [<CompiledName("PlaySuccess")>]
    member t.playSuccess() = t.play Success

    [<CompiledName("PlayHint")>]
    member t.playHint() = t.play Hint

    [<CompiledName("PlayError")>]
    member t.playError() = t.play Error
