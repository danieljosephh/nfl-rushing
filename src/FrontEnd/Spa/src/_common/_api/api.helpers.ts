import axios, {
  AxiosError,
  AxiosRequestConfig,
  AxiosResponse,
  Method
} from 'axios'
import { ApiError } from '../_models/ApiError.mdl'
//import { redirectToLoginOn401 } from './api.interceptors'

const axiosInstance = axios.create()
//axiosInstance.interceptors.response.use(undefined, redirectToLoginOn401)

const DEFAULT_HTTP_REQUEST_CONFIG = {
  withCredentials: true,
  headers: { Pragma: 'no-cache' }
}

function getBaseApiUrl() {
  // const BASE_URL_DEV = 'https://connect-localhost.montriumdev.com:8956/api/'
  // const BASE_URL_PROD = window.location.origin + '/api/'
  // return process.env.NODE_ENV === 'development' ? BASE_URL_DEV : BASE_URL_PROD

  return 'https://localhost:5001/api/'
}

export const BASE_API_URL = getBaseApiUrl()

export const UNKNOWN_ERROR = {
  translationKey: 'errors.generic.unknown',
  message: '',
  status: -1,
  detail: '',
  type: 'unknown'
}

export const NETWORK_ERROR = {
  translationKey: 'errors.generic.network',
  message: '',
  status: -1,
  detail: '',
  type: 'network'
}

export function mapToApiError(axiosError: AxiosError): ApiError {
  const error = axiosError?.response?.data
  if (error && (error.type || error.errorCode || axiosError.response?.status)) {
    const { detail, type, parameters, errorCode, message } = error
    return {
      translationKey: `errors.api.${axiosError.response!.status || ''}${type ?? errorCode ?? ''
        }`,
      status: axiosError.response!.status,
      detail,
      message,
      type,
      parameters,
      errorCode: `${axiosError.response!.status}` + `${error.errorCode || ''}`
    }
  }

  if (axiosError.message === 'Network Error') {
    return NETWORK_ERROR
  }
  return { ...UNKNOWN_ERROR, status: axiosError.response?.status ?? -1 }
}

async function call<T>(
  apiPath: string,
  requestConfig = {},
  version = 1
): Promise<T> {
  const request = {
    url: `${BASE_API_URL}v${version}.0/${apiPath}`,
    ...DEFAULT_HTTP_REQUEST_CONFIG,
    ...requestConfig
  }

  try {
    const response: AxiosResponse<T> = await axiosInstance(request)
    return response.data
  } catch (error) {
    throw mapToApiError(error as AxiosError<any>)
  }
}

function createRequest(httpVerb: Method) {
  return async function httpRequest<T>(
    apiPath: string,
    requestConfig?: AxiosRequestConfig,
    version?: number
  ): Promise<T> {
    return call(apiPath, { ...requestConfig, method: httpVerb }, version)
  }
}

export const api = {
  call,
  delete: createRequest('DELETE'),
  get: createRequest('GET'),
  post: createRequest('POST'),
  put: createRequest('PUT'),
  patch: createRequest('PATCH')
}
