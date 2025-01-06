import * as v from 'valibot'

export const joinCreateRoomSchema = v.object({
	nickname: v.pipe(v.string(), v.minLength(5, 'Minimum of 5 characters')),
	roomCode: v.pipe(v.string(), v.minLength(5, 'Minimum of 5 characters')),
})

export type JoinCreateRoom = v.InferOutput<typeof joinCreateRoomSchema>
