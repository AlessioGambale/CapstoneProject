=== path_choice_1 ===
# speaker: Orin
We must choose our next move carefully.
Which path do you take?
* [Left]
    # path: left
    # nextknot: path_left_2
    -> END
* [Right]
    # path: right
    # nextknot: path_right_2
    -> END

=== path_left_2 ===
# speaker: Orin
Left it is. And now?
* [Center]
    # path: center
    # nextknot: path_left_center_3
    -> END
* [Right]
    # path: right
    # nextknot: path_left_right_3
    -> END

=== path_left_center_3 ===
# speaker: Orin
Through the center. One last choice.
* [Left]
    # path: left
    -> END
* [Right]
    # path: right
    -> END

=== path_left_right_3 ===
# speaker: Orin
To the right. One last choice.
* [Center]
    # path: center
    -> END
* [Right]
    # path: right
    -> END

=== path_right_2 ===
# speaker: Orin
Right it is. And now?
* [Left]
    # path: left
    # nextknot: path_right_left_3
    -> END
* [Center]
    # path: center
    # nextknot: path_right_center_3
    -> END

=== path_right_left_3 ===
# speaker: Orin
To the left. One last choice.
* [Center]
    # path: center
    -> END
* [Right]
    # path: right
    -> END

=== path_right_center_3 ===
# speaker: Orin
Through the center. One last choice.
* [Center]
    # path: center
    -> END
* [Right]
    # path: right
    -> END