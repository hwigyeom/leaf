export type TypedEventHandler<T> = (args?: T) => any;
export type ParameterizedEventHandler = (...args: any[]) => any;

export interface IDisposable {
  dispose(): void;
}

/**
 * 이벤트 매개변수 형식의 설정이 가능한 이벤트
 */
export class TypedEvent<T> {
  private handlers: Array<TypedEventHandler<T>> = [];
  private onceHandlers: Array<TypedEventHandler<T>> = [];

  /**
   * 이벤트가 발생할때마다 실행되는 핸들러를 등록합니다.
   * @param {TypedEventHandler<T>} handler 이벤트 핸들러
   * @return {IDisposable} 등록된 핸들러를 제거할 수 있는 기능을 제공합니다.
   */
  public on(handler: TypedEventHandler<T>): IDisposable {
    this.handlers.push(handler);
    return {
      dispose: () => this.off(handler)
    };
  }

  /**
   * 핸들러가 등록된 이후 첫번째 발생 이벤트에만 실행되도록 등록합니다.
   * @param {TypedEventHandler<T>} handler 이벤트 핸들러
   */
  public once(handler: TypedEventHandler<T>): void {
    this.onceHandlers.push(handler);
  }

  /**
   * 등록된 이벤트 핸들러를 등록 해제합니다.
   * @param {TypedEventHandler<T>} handler 등록 해제할 이벤트 핸들러
   */
  public off(handler: TypedEventHandler<T>): void {
    const handlerIndex = this.handlers.indexOf(handler);
    if (handlerIndex > -1) {
      this.handlers.splice(handlerIndex, 1);
    }
  }

  /**
   * 이벤트 실행합니다.
   * @param {T} args 이벤트 핸들러로 전달할 매개변수입니다.
   */
  public emit(args: T): void {
    this.handlers.forEach((handler) => handler(args));
    this.onceHandlers.forEach((handler) => handler(args));
    this.onceHandlers = [];
  }

  /**
   * 이벤트 매개변수를 공유하는 연계된 이벤트를 설정합니다.
   * @param {TypedEvent<T>} event 연계 실행된 이벤트
   * @return {IDisposable} 연계 실행을 제거하는 기능을 제공합니다.
   */
  public pipe(event: TypedEvent<T>): IDisposable {
    return this.on((args) => event.emit(args));
  }
}

/**
 * 이벤트 매개변수를 갯수와 관계없이 자유롭게 전달할 수 있는 이벤트입니다.
 */
export abstract class ParameterizedEvent {
  private handlers: ParameterizedEventHandler[] = [];
  private onceHandlers: ParameterizedEventHandler[] = [];

  /**
   * 이벤트가 발생할때마다 실행되는 핸들러를 등록합니다.
   * @param {ParameterizedEventHandler} handler 이벤트 핸들러
   * @return {IDisposable} 등록된 핸들러를 제거할 수 있는 기능을 제공합니다.
   */
  public on(handler: ParameterizedEventHandler): IDisposable {
    this.handlers.push(handler);
    return {
      dispose: () => this.off(handler)
    };
  }

  /**
   * 핸들러가 등록된 이후 첫번째 발생 이벤트에서만 실행되는 등록합니다.
   * @param {ParameterizedEventHandler} handler
   */
  public once(handler: ParameterizedEventHandler): void {
    this.onceHandlers.push(handler);
  }

  /**
   * 등록된 이벤트 핸들러를 등록 해제합니다.
   * @param {ParameterizedEventHandler} handler 등록 해제할 이벤트 핸들러
   */
  public off(handler: ParameterizedEventHandler): void {
    const handlerIndex = this.handlers.indexOf(handler);
    if (handlerIndex > -1) {
      this.handlers.splice(handlerIndex, 1);
    }
  }

  /**
   * 이벤트를 실행합니다.
   * @param args 이벤트 핸들러로 등록할 매개변수 입니다.
   */
  public emit(...args: any[]): void {
    this.handlers.forEach((handler) => handler(...args));
    this.handlers.forEach((handler) => handler(...args));
    this.onceHandlers = [];
  }

  /**
   * 이벤트 매개변수를 공유하는 연계된 이벤트를 설정합니다.
   * @param {ParameterizedEvent} event 연계 실행될 이벤트
   * @return {IDisposable} 연계 실행을 제거하는 기능을 제공합니다.
   */
  public pipe(event: ParameterizedEvent): IDisposable {
    return this.on((...args: any[]) => event.emit(...args));
  }
}
