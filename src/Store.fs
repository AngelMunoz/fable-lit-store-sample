module Lit.Store

open Lit
open Fable
open Fable.Core
open Fable.Core.JsInterop

type ILitStore<'Type> =
    inherit IStore<'Type>
    abstract member Value: 'Type
    abstract member SetValue: 'Type -> unit

[<AttachMembers>]
type LitStore<'Type>(host: LitElement, initial: 'Type) as this =
    let store = new Store<'Type>(initial)
    let mutable value = initial

    do host?addController (this)

    member private _.hostConnected() =
        store.Subscribe(fun newValue ->
            value <- newValue
            host.requestUpdate ())
        |> ignore

    member private _.hostDisconnected() = store.Dispose()

    interface ILitStore<'Type> with
        member _.Value = value

        member _.Update(updateFn: 'Type -> 'Type) = store.Update(updateFn)

        member _.SetValue(value: 'Type) = store.Update(fun _ -> value)

let (<~) (store: ILitStore<'Type>) (value: 'Type) = store.SetValue(value)


[<RequireQualifiedAccess>]
module LitStore =

    let make<'Type> (initialValue: 'Type) (host: LitElement) =
        LitStore(host, initialValue) :> ILitStore<'Type>

    let update (mapFn: 'Type -> 'Type) (store: ILitStore<'Type>) = store.Update mapFn

    let set value (store: ILitStore<'Type>) = store.SetValue value

    let value (store: ILitStore<'Type>) = store.Value
