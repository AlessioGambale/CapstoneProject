Path

-> path_choice_1
=== path_choice_1 ===
# speaker: Orin
We must choose our next move carefully.
Which path do you take?
* [Left]
    -> path_left_2
* [Right]
    -> path_right_2

=== path_left_2 ===
# speaker: Orin
Left it is. And now?
* [Center]
    -> path_left_center_3
* [Right]
    -> path_left_right_3

=== path_left_center_3 ===
# speaker: Orin
Through the center. One last choice.
* [Left]
    # path: left_center_left
    -> END
* [Right]
    # path: left_center_right
    -> END

=== path_left_right_3 ===
# speaker: Orin
To the right. One last choice.
* [Center]
    # path: left_right_center
    -> END
* [Right]
    # path: left_right_right
    -> END

=== path_right_2 ===
# speaker: Orin
Right it is. And now?
* [Left]
    -> path_right_left_3
* [Straight]
    -> path_right_straight_3

=== path_right_left_3 ===
# speaker: Orin
To the left. One last choice.
* [Straight]
    # path: right_left_straight
    -> END
* [Right]
    # path: right_left_right
    -> END

=== path_right_straight_3 ===
# speaker: Orin
Straight ahead. One last choice.
* [Straight]
    # path: right_straight_straight
    -> END
* [Right]
    # path: right_straight_right
    -> END
