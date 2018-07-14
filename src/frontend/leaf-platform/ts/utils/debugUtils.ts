import { UrlService } from '../network/UrlService';

export function log(message: string, ...args: any[]): void {
  if (UrlService.isDebugMode) {
    console.log(message, ...args);
  }
}

export function info(message: string, ...args: any[]): void {
  if (UrlService.isDebugMode) {
    console.info(message, ...args);
  }
}

export function warn(message: string, ...args: any[]): void {
  if (UrlService.isDebugMode) {
    console.warn(message, ...args);
  }
}

export function error(message: string, ...args: any[]): void {
  if (UrlService.isDebugMode) {
    console.error(message, ...args);
  }
}

export function table(data: any, columns?: string[]): void {
  if (UrlService.isDebugMode) {
    if (!console.table) {
      console.log(data);
    } else {
      console.table(data, columns);
    }
  }
}
