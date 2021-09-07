// import { Dictionary } from 'types'

export type ApiError = {
  translationKey: string
  status: number
  detail: string
  message: string
  type?: string
  parameters?: Dictionary<string>
  errorCode?: string
}

// this type is normally kept here: c:/dev/Connect/src/FrontEnd/Spa/src/types/index
export type Dictionary<T> = { [id: string]: T }