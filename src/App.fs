[<RequireQualifiedAccess>]
module App

open Fable
open Fable.Core
open Lit
open Fable.Core.JsInterop
open Browser.Types
open Types
open Browser.Dom

open Lit.Store


let mutable intStore: ILitStore<int> option = None

[<LitElement("root-app")>]
let private app () =
    let host = LitElement.init ()
    // temporary workaround
    Hook.useEffectOnce (fun () -> intStore <- host |> LitStore.make 0 |> Some)
    // temporary workaround

    let reset _ =
        match intStore with
        | Some store -> store |> LitStore.set 0
        | None -> ()

    let increment _ =
        match intStore with
        | Some store -> store |> LitStore.update (fun value -> value + 1)
        | None -> ()

    let decrement _ =
        match intStore with
        | Some store -> store |> LitStore.update (fun value -> value - 1)
        | None -> ()

    let value =
        match intStore with
        | Some store -> store |> LitStore.value
        | None -> 0

    html
        $"""
        <p>{value}</p>
        <button @click={increment}>Increment</button>
        <button @click={decrement}>Decrement</button>
        <button @click={reset}>Reset</button>
    """

let register () = ()
