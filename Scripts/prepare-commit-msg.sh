#!/bin/sh

COMMIT_EDITMSG=$1

TAGS=""

Files=$(git diff --name-only --cached)
for Line in $Files;
do
    tag=''
    tag=$(python .git/hooks/getPrefixFromPath.py $Line)
    len=`echo $TAGS |awk '{print length}'`
    if [ $len -gt 1 ]; then
        TAGS+=","
    fi
    TAGS+="$tag";
done

printf "[$TAGS] $(cat $COMMIT_EDITMSG)" > $COMMIT_EDITMSG