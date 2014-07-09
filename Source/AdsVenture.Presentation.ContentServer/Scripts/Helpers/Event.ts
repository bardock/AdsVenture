module Helpers {

    export interface ITEvent<TArg> {
        on(handler: (data: TArg) => void): ITEvent<TArg>;
        off(handler: (data: TArg) => void): ITEvent<TArg>;
    }

    export interface IEvent extends ITEvent<any> { }

    export class TEvent<TArg> implements ITEvent<TArg> {
        handlers: { (data: TArg): void; }[] = [];

        public on(handler: (data: TArg) => void): IEvent {
            this.handlers.push(handler);
            return this;
        }

        public off(handler: (data: TArg) => void): IEvent {
            this.handlers = this.handlers.filter(h => h !== handler);
            return this;
        }

        public trigger(data: TArg): IEvent {
            if (this.handlers) {
                this.handlers.forEach(h => h(data));
            }
            return this;
        }
    }

    export class Event<TArg> extends TEvent<any> { }
}