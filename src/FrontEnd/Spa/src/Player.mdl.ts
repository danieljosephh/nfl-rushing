export type Player = {
  name: string
  team: string
  position: string
  attempts: number
  attemptsPerGame: number
  yards: number
  yardsPerAttempt: number
  yardsPerGame: number
  touchdowns: number
  longestRush: number
  wasLongestRushATouchdown: boolean,
  firstDowns: number
  firstDownsPercentage: number
  twentyPlus: number
  fortyPlus: number
  fumbles: number
}