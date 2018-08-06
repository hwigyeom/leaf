import 'reflect-metadata';
import { IView } from '../views/IView';

export function ResolveComponent(id: string) {
  return (target: any, propertyKey: string) => {
    const type = Reflect.getMetadata('design:type', target, propertyKey);
    let component;

    if (delete target[propertyKey]) {
      Object.defineProperty(target, propertyKey, {
        get() {
          const view: IView = this as IView;
          if (view) {
            if (component) {
              return component;
            }

            const $el = $(`#${id}[data-role=leaf-component]`, view['_container_']);
            const result = $el.data('component');

            if (result instanceof type) {
              return component = result;
            }
          }

          throw new Error(`id가 '${id}' 인 '${type.name}' 형식의 컴포넌트를 찾을 수 없습니다.`);
        }
      });
    }
  };
}
