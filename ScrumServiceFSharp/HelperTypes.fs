namespace ScrumServiceFSharp.HelperTypes

open System

exception InvalidLength of string 


module String100 =
    type String100 = string
    let createString100 (s: string) =
        if s <> null && s.Length <= 100
        then String100 s
        else raise (InvalidLength("Invalid length"))
    let applyString100 f s = f s
    let valueString100 s = applyString100 id s

