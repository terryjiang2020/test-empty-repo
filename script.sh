#!/usr/bin/env expect
set timeout -1

# Function to handle interactive prompts
proc handle_prompts {} {
    set prev_lines [list "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" "" ""]
    # Store all output for final processing
    set all_output ""
    set last_processed_lines ""
    # 新增标志，跟踪是否已发送"enter"
    set sent_enter 0
    set trigger_count 0
    global check_count 0
    # 新增计数器，跟踪更新次数
    set update_count 0
    # 初始检查行数，每次enter增加1
    set lines_to_check 5
    # Flag to track if we've seen the white ⏺ symbol
    set seen_white_symbol 0
    # Flag to track if we've seen the input field
    set seen_input_field 0
    global no_update_count 0

    proc exit_process {} {
        # Extract text between the last white ⏺ symbol and the input field (╭───)
        if {[regexp {.*\x1B\[38;2;255;255;255m⏺\x1B\[39m(.*?)╭} $all_output -> extracted_content]} {
            # Remove ANSI escape sequences from the extracted text
            # Use a more comprehensive regex to handle all ANSI styles
            regsub -all {\x1B\[[0-9;]*[a-zA-Z]} $extracted_content "" clean_text
            # Trim leading/trailing whitespace
            set clean_text [string trim $clean_text]
            puts "Successfully extracted text between white ⏺ and input field"
            
            # Filter out colored lines like "· Puttering..." which are not white
            set filtered_lines [list]
            foreach line [split $clean_text "\n"] {
                # Skip lines containing "Puttering", "Deliberating", or other colored status indicators
                if {![regexp {·\s+Puttering|✽\s+Deliberating|✻\s+Deliberating|✢\s+Deliberating|✳\s+Deliberating|∗\s+Deliberating|·\s+Deliberating} $line]} {
                    lappend filtered_lines $line
                }
            }
            set clean_text [join $filtered_lines "\n"]
            set clean_text [string trim $clean_text]
            
            # set file [open "extracted_response.txt" "w"]
            # puts $file $clean_text
            # close $file
            
            # Force immediate exit
            puts "Content extracted successfully. Forcing exit..."
        } else {
            puts "Could not find text between the last white ⏺ and input field"
        }

        puts "Exiting with exit code 0..."
        # Make sure to force exit
        exit 0
    }

    # 定义一个定时器来检查是否有更新
    proc check_no_update {} {
        after 1000 {
            incr no_update_count
            if {$no_update_count >= 10 && $check_count > 20} {
                puts "No updates detected for 10 seconds. Exiting..."
                exit 0
            } else {
                puts "No updates detected for $no_update_count seconds. Waiting..."
                check_no_update
            }
        }
    }

    # Function to reset the timer
    proc reset_timer {} {
        global timer_id no_update_count
        # Cancel the existing timer
        if {[info exists timer_id]} {
            after cancel $timer_id
        }
        # Reset the no_update_count and restart the timer
        set no_update_count 0
    }

    # 启动定时器
    check_no_update

    expect {
        -re ".*\\S.*" {
            # 每次检测到变化时重置定时器
            reset_timer
            set last_line $expect_out(buffer)
            incr check_count
            lappend prev_lines $last_line
            # Keep (lines_to_check) lines, which increases each time enter is sent
            set prev_lines [lrange $prev_lines end-$lines_to_check end]
            set combined_lines [join $prev_lines "\n"]
            
            # Append to all_output for final extraction
            append all_output "$last_line\n"

            # Record current output for debugging
            # set file [open "last_detected_lines.txt" "w"]
            # puts $file $combined_lines
            # close $file

            if {$check_count > 20} {
                # Check for white ⏺ symbol in the current line
                if {[regexp {\x1B\[38;2;255;255;255m⏺\x1B\[39m} $last_line]} {
                    puts "Detected white ⏺ symbol!"
                    set seen_white_symbol 1
                }

                # Check for input field pattern - using a more lenient pattern
                # Only care if the input field exists, don't check for white > yet
                if {[regexp {.*╭.*\x1B\[39m\x1B\[22m >.*} $combined_lines]} {
                    puts "Detected input field with prompt!"
                    set seen_input_field 1
                }
            }

            # 只有当输出变化且未发送"enter"时才处理
            if { $combined_lines ne $last_processed_lines && !$sent_enter &&
                [regexp {.*❯.*} $combined_lines] } {
                    puts "Detected new prompt: $combined_lines"
                    set last_processed_lines $combined_lines
                    incr trigger_count
                    puts "Sending enter..."
                    sleep 1
                    send "\r"
                    sleep 1
                    send "\b"
                    sleep 1
                    set sent_enter 1  ;# 设置标志，表示已发送"enter"
                    set update_count 0;  # 重置更新计数器
                    
                    # 增加检查行数，最多检查30行（考虑到初始化的prev_lines大小）
                    if { $lines_to_check < 30 } {
                        incr lines_to_check
                        puts "Increased lines to check to $lines_to_check"
                    }
                    sleep 1
            } elseif { $combined_lines ne $last_processed_lines } {
                # # Continue processing even if the > symbol hasn't changed to white
                # # Only check if we've seen the white ⏺ symbol and any input field pattern
                # if {$seen_white_symbol && $seen_input_field && $check_count > 20} {
                #     puts "Both white ⏺ symbol AND input field pattern detected. Attempting to extract text..."
                    
                #     # Save all output for debugging
                #     # set file [open "all_response.txt" "w"]
                #     # puts $file $all_output
                #     # close $file
                    
                    # # Extract text between the last white ⏺ symbol and the input field (╭───)
                    # if {[regexp {.*\x1B\[38;2;255;255;255m⏺\x1B\[39m(.*?)╭} $all_output -> extracted_content]} {
                    #     # Remove ANSI escape sequences from the extracted text
                    #     # Use a more comprehensive regex to handle all ANSI styles
                    #     regsub -all {\x1B\[[0-9;]*[a-zA-Z]} $extracted_content "" clean_text
                    #     # Trim leading/trailing whitespace
                    #     set clean_text [string trim $clean_text]
                    #     puts "Successfully extracted text between white ⏺ and input field"
                        
                    #     # Filter out colored lines like "· Puttering..." which are not white
                    #     set filtered_lines [list]
                    #     foreach line [split $clean_text "\n"] {
                    #         # Skip lines containing "Puttering", "Deliberating", or other colored status indicators
                    #         if {![regexp {·\s+Puttering|✽\s+Deliberating|✻\s+Deliberating|✢\s+Deliberating|✳\s+Deliberating|∗\s+Deliberating|·\s+Deliberating} $line]} {
                    #             lappend filtered_lines $line
                    #         }
                    #     }
                    #     set clean_text [join $filtered_lines "\n"]
                    #     set clean_text [string trim $clean_text]
                        
                    #     # set file [open "extracted_response.txt" "w"]
                    #     # puts $file $clean_text
                    #     # close $file
                        
                    #     # Force immediate exit
                    #     puts "Content extracted successfully. Forcing exit..."
                    # } else {
                    #     puts "Could not find text between the last white ⏺ and input field"
                    # }
                    
                #     puts "Exiting with exit code 0..."
                #     # Make sure to force exit
                #     exit 0
                # } else {
                #     # Output changed but not all exit conditions met
                #     puts "Output changed, resetting sent_enter... (seen_white_symbol: $seen_white_symbol, seen_input_field: $seen_input_field)"
                #     set sent_enter 0
                # }
                # Output changed but not all exit conditions met
                puts "Output changed, resetting sent_enter... (seen_white_symbol: $seen_white_symbol, seen_input_field: $seen_input_field)"
                set sent_enter 0
            } else {
                puts "Lines unchanged, waiting..."
            }

            # 如果已发送"enter"，增加更新计数器
            if { $sent_enter } {
                incr update_count
                if { $update_count >= 5 } {
                    puts "Resetting sent_enter after 5 updates..."
                    set sent_enter 0
                }
            }

            exp_continue
        }
        timeout {
            puts "Timeout waiting for prompt"
            exit 1
        }
        eof {
            set combined_lines [join $prev_lines "\n"]
            if { $combined_lines ne "" } {
                puts "Detected last lines: $combined_lines"
                # set file [open "last_detected_lines.txt" "w"]
                # puts $file $combined_lines
                # close $file
            }
        }
    }
}


# 运行命令
spawn claude "do what prompt.txt asks you to do, print your response in response.txt, and print the cost of the session in cost.txt"
# spawn claude --verbose "Write a unit test for the src/constants.js file"
handle_prompts